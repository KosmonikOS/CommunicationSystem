using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using Microsoft.AspNetCore.Http.Features;

namespace CommunicationSystem.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment env;
        private readonly FormOptions formOptions;
        private readonly PathOptions pathOptions;

        public FileService(IHostingEnvironment environment
            , IOptions<PathOptions> pathOptions, IOptions<FormOptions> formOptions)
        {
            this.env = environment;
            this.formOptions = formOptions.Value;
            this.pathOptions = pathOptions.Value;
        }
        public bool IsImage(string fileName)
        {
            return MimeMapping.MimeUtility.GetMimeMapping(fileName).StartsWith("image/");
        }
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var path = pathOptions.UploadFolder + DateTime.Now.TimeOfDay.TotalMilliseconds + file.FileName.ToString();
            using (var filestr = new FileStream(env.ContentRootPath + pathOptions.UploadPath + path, FileMode.Create))
            {
                await file.CopyToAsync(filestr);
            }
            return path;
        }
        public async Task<FormValueProvider> SaveStreamFileWithFormDataAsync(HttpRequest request)
        {
            if (!IsMultipartContentType(request.ContentType))
            {
                throw new Exception($"Expected a multipart request, but got {request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = GetBoundary(
                MediaTypeHeaderValue.Parse(request.ContentType),
                formOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (HasFileContentDisposition(contentDisposition))
                    {
                        var path = pathOptions.UploadFolder + DateTime.Now.TimeOfDay.TotalMilliseconds
                            + contentDisposition.FileName.Value;
                        using (var filestr = new FileStream(env.ContentRootPath + pathOptions.UploadPath + path, FileMode.Create))
                        {
                            await section.Body.CopyToAsync(filestr);
                        }
                        formAccumulator.Append("Content", path);
                        formAccumulator.Append("FileType", Path.GetExtension(contentDisposition.FileName.Value));
                    }
                    else if (HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.Value, value);

                            if (formAccumulator.ValueCount > formOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {formOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);

            return formValueProvider;
        }
        private Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
        private string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            //var boundary = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(contentType.Boundary);// .NET Core <2.0
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                    && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        private bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                        || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}

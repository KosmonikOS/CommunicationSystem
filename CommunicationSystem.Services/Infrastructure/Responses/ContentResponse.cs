using CommunicationSystem.Services.Infrastructure.Enums;

namespace CommunicationSystem.Services.Infrastructure.Responses
{
    public class ContentResponse<TContent> : BaseResponse, IContentResponse<TContent>
    {
        public ContentResponse(ResponseStatus status) : base(status)
        {
        }
        public TContent Content { get; set; }
    }
}

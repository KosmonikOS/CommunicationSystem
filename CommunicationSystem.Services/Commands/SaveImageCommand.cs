using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Commands
{
    public class SaveImageCommand :IRequest<IContentResponse<string>>
    {
        public IFormFile File { get; set; }
    }
}

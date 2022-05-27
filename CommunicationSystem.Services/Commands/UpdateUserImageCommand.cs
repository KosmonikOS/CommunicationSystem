using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateUserImageCommand :IRequest<IContentResponse<string>>
    {
        public int Id { get; set; }
        public IFormFile ImageToSave { get; set; }
    }
}

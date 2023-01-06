using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationSystem.Services.Commands
{
    public class AddFileMessageCommand :IRequest<IContentResponse<FileMessageResponseDto>>
    {
        public ControllerBase ControllerContext { get; set; } //Use context to obtain save file and obtain model from form
    }
}

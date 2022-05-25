using CommunicationSystem.Services.Infrastructure.Enums;

namespace CommunicationSystem.Services.Infrastructure.Responses
{
    public interface IResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; }
        public int StatusCode { get; }

    }
}

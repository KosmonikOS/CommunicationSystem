using CommunicationSystem.Services.Infrastructure.Enums;

namespace CommunicationSystem.Services.Infrastructure.Responses
{
    public class BaseResponse : IResponse
    {
        public BaseResponse(ResponseStatus status)
        {
            Status = status;
        }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => Status == ResponseStatus.Ok;
        public int StatusCode => (int)Status;
    }
}

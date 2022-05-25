namespace CommunicationSystem.Services.Infrastructure.Responses
{
    public interface IContentResponse<TContent> :IResponse
    {
        public TContent Content { get; set; }
    }
}


namespace CommunicationSystem.Domain.Entities
{
    public class UserLastMessage
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string accountImage { get; set; }
        public string Email{get;set;}
        public long MessageId { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public int NotViewed { get; set; }
        public string UserActivity { get; set; }
    }
}

using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Services.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CommunicationContext context;

        public MessageRepository(CommunicationContext context)
        {
            this.context = context;
        }

        public void AddMessage(Message message)
        {
            context.Add(message);
        }
        public void UpdateMessageContent(int id, string content)
        {
            var message = context.Messages
                .FirstOrDefault(x => x.Id == id);
            message.Content = content;
        }

        public void DeleteMessage(int id)
        {
            var message = context.Messages
                .FirstOrDefault(x => x.Id == id);
            context.Remove(message);
        }
        public void ViewMessage(int id)
        {
            var message = context.Messages
                .FirstOrDefault(x => x.Id == id);
            message.ViewStatus = ViewStatus.isViewed;
        }
        public IQueryable<GroupMessageDto> GetGroupMessages(int userId, Guid groupId, int page)
        {
            return context.Messages
                .Include(x => x.From)
                .Where(x => x.ToGroup == groupId)
                .OrderByDescending(x => x.Id).Skip(100 * page).Take(100)
                .Select(x => new GroupMessageDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    Date = x.Date,
                    Type = x.Type,
                    NickName = x.From.NickName,
                    AccountImage = x.From.AccountImage,
                    IsMine = x.FromId == userId
                }).OrderBy(x => x.Id);
        }

        public IQueryable<ContactMessageDto> GetMessagesBetweenContacts(int userId, int contactId, int page)
        {
            return context.ContactMessages.FromSqlRaw($"WITH \"u\" AS (UPDATE \"Messages\" AS \"um\" SET \"ViewStatus\" = 1 FROM " +
                  $"(SELECT \"Id\",\"Content\",\"Date\",\"Type\",(\"FromId\" = {userId}) AS \"IsMine\" FROM \"Messages\" WHERE " +
                  $"(\"FromId\" = {userId} and \"ToId\" = {contactId}) or (\"FromId\" = {contactId} and \"ToId\" = {userId}) " +
                  $"ORDER BY \"Id\" DESC OFFSET {100 * page} LIMIT 100) as \"m\" " +
                  $"WHERE \"um\".\"Id\" = \"m\".\"Id\" RETURNING \"m\".*) " +
                  $"SELECT * FROM \"u\" ORDER BY \"Id\" ");
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}

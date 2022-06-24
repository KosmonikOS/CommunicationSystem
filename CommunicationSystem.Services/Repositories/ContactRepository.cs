using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly CommunicationContext context;

        public ContactRepository(CommunicationContext context)
        {
            this.context = context;
        }

        public ContactDto GetGroupContact(Guid fromGroup)
        {
            return context.Groups.AsNoTracking()
                        .Select(x => new ContactDto()
                        {
                            ToGroup = x.Id,
                            IsGroup = true,
                            AccountImage = x.GroupImage,
                            NickName = x.Name
                        }).FirstOrDefault(x => x.ToGroup == fromGroup);
        }

        public ContactDto GetContact(int from)
        {
            return context.Users.AsNoTracking()
                        .Select(x => new ContactDto()
                        {
                            ToId = x.Id,
                            IsGroup = false,
                            AccountImage = x.AccountImage,
                            NickName = x.NickName,
                            LastActivity = UserFunctions.GetUserLastActivity((int)from),
                        }).FirstOrDefault(x => x.ToId == from);
        }

        public IQueryable<ContactDto> GetUserContacts(int userId)
        {
            return context.Contacts.FromSqlRaw($"(WITH \"m\" AS (SELECT DISTINCT ON (CASE WHEN \"FromId\" > \"ToId\" THEN (\"FromId\",\"ToId\") ELSE (\"ToId\",\"FromId\") END) " +
                      $"\"m\".\"Id\",(CASE WHEN \"FromId\" = {userId} THEN \"ToId\" ELSE \"FromId\" END) AS \"ToId\", null::uuid AS \"ToGroup\", " +
                      $"false AS \"IsGroup\",\"Content\" AS \"LastMessage\", \"Date\" AS \"LastMessageDate\",\"Type\" AS \"LastMessageType\", " +
                      $"\"GetNotViewedMessages\"(\"FromId\",{userId}) AS \"NotViewedMessages\" FROM \"Messages\" AS \"m\" " +
                      $"WHERE NOT \"IsGroup\" AND (\"FromId\" = {userId} OR \"ToId\" = {userId}) " +
                      $"ORDER BY (CASE WHEN \"FromId\" > \"ToId\" THEN (\"FromId\",\"ToId\") ELSE (\"ToId\",\"FromId\") END),\"Id\" DESC) " +
                      $"SELECT \"m\".*,\"u\".\"NickName\",\"u\".\"AccountImage\",\"GetUserLastActivity\"(\"ToId\") AS \"LastActivity\" FROM \"m\" " +
                      $"INNER JOIN \"Users\" AS \"u\" ON \"u\".\"Id\" = \"m\".\"ToId\") " +
                      $"UNION " +
                      $"(SELECT \"m\".\"Id\",null AS \"ToId\",\"GroupId\" AS \"ToGroup\",true AS \"IsGroup\", \"m\".\"Content\" AS \"LastMessage\", " +
                      $"\"m\".\"Date\" AS \"LastMessageDate\", \"Type\" AS \"LastMessageType\" ,0 AS \"NotViewedMessages\",\"g\".\"Name\" AS \"NickName\", " +
                      $"\"g\".\"GroupImage\" AS \"AccountImage\", null AS \"LastActivity\" FROM \"GroupUser\" AS \"gu\" " +
                      $"INNER JOIN \"Groups\" AS \"g\" ON \"g\".\"Id\" = \"gu\".\"GroupId\" " +
                      $"LEFT JOIN \"Messages\" AS \"m\" ON \"gu\".\"GroupId\" = \"m\".\"ToGroup\" " +
                      $"WHERE \"gu\".\"UserId\" = {userId}  AND (\"m\".\"Id\" IS NULL OR \"m\".\"Id\" = " +
                      $"(SELECT MAX(\"Id\") FROM \"Messages\" WHERE \"ToGroup\" = \"gu\".\"GroupId\"))) " +
                      $"ORDER BY \"Id\" DESC NULLS LAST ");
        }
    }
}

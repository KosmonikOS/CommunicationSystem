using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IAuthRepository : IBaseRepository
    {
        public EntityEntry<User> SetTime(UserActivityDto dto);
        public IContentResponse<User> GetConfirmedUser(LoginDto dto);
    }
}

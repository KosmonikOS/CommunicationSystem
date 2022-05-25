using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IAuthRepository : IBaseRepository
    {
        public EntityEntry<User> SetTime(int id, UserActivityState act);
        public IContentResponse<User> GetConfirmedUser(LoginDto dto);
    }
}

using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IStudentRepository :IBaseRepository
    {
        public IQueryable<User> GetStudents(string search, StudentsSearchOption searchOption);
        public IQueryable<TestUser> GetStudents(Guid id);
        public IQueryable<StudentAnswerDto> GetStudentAnswers(int userId, Guid testId);
        public IResponse UpdateStudentMark(UpdateStudentMarkDto dto);
        public void UpdateTestStudents(IEnumerable<TestStudentStateDto> students,Guid testId);
    }
}

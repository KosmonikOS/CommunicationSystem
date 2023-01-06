using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, IContentResponse<GroupShowDto>>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public GetGroupByIdQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<GroupShowDto>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = mapper.ProjectTo<GroupShowDto>(groupRepository
                .GetGroups(x => x.Id == request.Id)).FirstOrDefault();
            if (dto == null)
                return new ContentResponse<GroupShowDto>(ResponseStatus.NotFound) { Message = "Этой группы не существует" };
            return new ContentResponse<GroupShowDto>(ResponseStatus.Ok) { Content = dto };
        }
    }
}

using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand, IResponse>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public AddGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }
        public async Task<IResponse> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            var group = mapper.Map<Group>(request.Dto);
            group.Id = Guid.NewGuid();
            groupRepository.AddGroup(group);
            groupRepository.UpdateGroupMembers(request.Dto.Members, group.Id);
            await groupRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}

using ARM.Areas.Tutor.Models.Group;
using ARM.Data.Entities;
using AutoMapper;

namespace ARM.Areas.Tutor.Mappers
{
    public class GroupMapper : Profile
    {
        public GroupMapper()
        {
            CreateMap<GroupEntity, GroupInfoViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name));
        }
    }
}

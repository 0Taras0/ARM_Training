using ARM.Areas.Tutor.Models.Group;
using ARM.Areas.Tutor.Models.Subject;
using ARM.Data.Entities;
using AutoMapper;

namespace ARM.Areas.Tutor.Mappers
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<SubjectEntity, SubjectInfoViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.GroupId, opt => opt.MapFrom(x => x.GroupId));
        }
    }
}

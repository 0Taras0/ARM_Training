using ARM.Data.Entities;
using ARM.Data.Entities.Identity;
using ARM.Models.Accounts;
using ARM.Models.Subject;
using AutoMapper;

namespace ARM.Mappers
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<SubjectEntity, SubjectViewModel>()
                .ForMember(x => x.Grades, opt => opt.Ignore())
                .ForMember(x => x.GroupName, opt => opt.MapFrom(x => x.Group.Name))
                .ForMember(x => x.TutorName, opt => opt.Ignore());
        }
    }
}

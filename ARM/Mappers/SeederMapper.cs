using ARM.Data.Entities.Identity;
using ARM.Models.Seeder;
using AutoMapper;

namespace ARM.Mappers
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}

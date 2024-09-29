using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.ClassModelViews;

namespace OnDemandTutor.Repositories.Mappers
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<CreateClassModelView, Class>()
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());
            CreateMap<UpdateClassModelView, Class>()
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());
            CreateMap<Class, ResponseClassModelView>();
        }
    }
}

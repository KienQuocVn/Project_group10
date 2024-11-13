using AutoMapper;
using OnDemandTutor.ModelViews.SubjectModelViews;
using OnDemandTutor.Contract.Repositories.Entity;

namespace OnDemandTutor.Repositories.Mappers
{
    internal class SubjectMapping : Profile
    {
        public SubjectMapping()
        {
            CreateMap<CreateSubjectModelViews, Subject>()
                .ForMember(dest => dest.TutorId, opt => opt.Ignore()) 
                .ForMember(dest => dest.TutorSubjects, opt => opt.Ignore())
                .ForMember(dest => dest.Classes, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())); 

            CreateMap<UpdateSubjectModel, Subject>()
                .ForMember(dest => dest.TutorId, opt => opt.Ignore())  
                .ForMember(dest => dest.TutorSubjects, opt => opt.Ignore())
                .ForMember(dest => dest.Classes, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        }
    }
}
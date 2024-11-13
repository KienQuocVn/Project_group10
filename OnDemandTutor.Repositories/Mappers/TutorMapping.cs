using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;

namespace OnDemandTutor.Repositories.Mappers
{
    public class Tutormapping : Profile
    {
        public Tutormapping()
        {
            CreateMap<TutorSubject, ResponseTutorModelViews>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdateTime, opt => opt.Ignore());

            CreateMap<CreateTutorSubjectModelViews, TutorSubject>()
                .ForMember(dest => dest.TutorId, opt => opt.Ignore())  
                .ForMember(dest => dest.Bookings, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore());

            CreateMap<UpdateTutorSubjectModelViews, TutorSubject>()
                .ForMember(dest => dest.TutorId, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        }
    }
}
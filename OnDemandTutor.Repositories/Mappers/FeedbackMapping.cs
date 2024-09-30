using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.FeedbackModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.Mappers
{
    public class FeedbackMapping : Profile
    {
        public FeedbackMapping() {
            CreateMap<CreateFeedbackModelViews , Feedback>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N")))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTimeOffset.Now))
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => DateTimeOffset.Now));
            
            CreateMap<UpdateFeedbackModelViews , Feedback>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N")))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTimeOffset.Now))
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => DateTimeOffset.Now));
        }
    }
}

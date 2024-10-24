using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.ClassModelViews;
using OnDemandTutor.ModelViews.RequestRefundModelViews;


namespace OnDemandTutor.Repositories.Mappers
{
    public class RequestRefundMapping : Profile
    {
        public RequestRefundMapping() {
            CreateMap<CreateRequestRefundModelViews, RequestRefund>()
               .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
               .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
               .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());
            CreateMap<RequestRefund, ResponseRequestRefundModelViews>();

        }
    }
}

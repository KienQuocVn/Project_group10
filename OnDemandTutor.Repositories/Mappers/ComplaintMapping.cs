using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.ComplaintModelViews;

namespace OnDemandTutor.Repositories.Mappers
{
    public class ComplaintMapping : Profile
    {
        public ComplaintMapping()
        {
            // Cấu hình ánh xạ từ Complaint sang ResponseComplaintModel
            CreateMap<Complaint, ResponseComplaintModel>();

            // Cấu hình ánh xạ từ CreateComplaintModel sang Complaint
            CreateMap<CreateComplaintModel, Complaint>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Bỏ qua CreatedAt
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending")); // Gán trạng thái mặc định là "Pending"

            // Cấu hình ánh xạ từ UpdateComplaintModel sang Complaint
            CreateMap<UpdateComplaintModel, Complaint>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Bỏ qua CreatedAt
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // Bỏ qua Status
        }
    }
}

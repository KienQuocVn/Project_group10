using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.ScheduleModelViews;

namespace OnDemandTutor.Repositories.Mappers
{
    public class ScheduleMapping : Profile
    {
        public ScheduleMapping()
        {
            // Cấu hình ánh xạ từ Schedule sang ResponseScheduleModelViews
            CreateMap<Schedule, ResponseScheduleModelViews>();

            // Cấu hình ánh xạ từ CreateScheduleModelViews sang Schedule
            CreateMap<CreateScheduleModelViews, Schedule>()
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());

            // Cấu hình ánh xạ từ UpdateScheduleModelViews sang Schedule
            CreateMap<UpdateScheduleModelViews, Schedule>()
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());
        }
    }
}

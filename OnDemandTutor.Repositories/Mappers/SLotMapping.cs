using AutoMapper;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.SLotModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.Mappers
{
    public class SLotMapping : Profile
    {
        public SLotMapping() {
            CreateMap<Slot, SlotModelView>().ReverseMap();
        }    
    }
}

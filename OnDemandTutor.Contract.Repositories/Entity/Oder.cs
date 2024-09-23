using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Oder : BaseEntity
    {

        public String Type { set; get; }
        public String Name { set; get; }
        public double Total { get; set; }
        public Guid AccountId { set; get; }
        public String SubjectId { set; get; }
        public String Status { set; get; }

        public virtual Accounts Accounts { get; set; }
        public virtual Subject Subject { get; set; }

    }
}

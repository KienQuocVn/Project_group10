using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.AuthModelViews
{
    public class UpdateClaimModel
    {
        public Guid ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UpdatedBy { get; set; }
    }
}

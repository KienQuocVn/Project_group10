﻿using Microsoft.AspNetCore.Identity;
using OnDemandTutor.Core.Utils;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class ApplicationUserClaims : IdentityUserClaim<Guid>
    {
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public ApplicationUserClaims()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}

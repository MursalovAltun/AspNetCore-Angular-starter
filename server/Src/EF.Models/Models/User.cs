using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EF.Models.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; } = "en";
        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<PushSubscription> PushSubscriptions { get; set; } = new List<PushSubscription>();

        public virtual ICollection<UserAccessFailedAttempt> UserAccessFailedAttempts { get; set; } =
            new List<UserAccessFailedAttempt>();

        public virtual ICollection<WebauthnCredential> WebauthnCredentials { get; set; } =
            new List<WebauthnCredential>();

        public virtual RegisterOptions RegisterOptions { get; set; }
        public virtual LoginOptions LoginOptions { get; set; }
    }
}
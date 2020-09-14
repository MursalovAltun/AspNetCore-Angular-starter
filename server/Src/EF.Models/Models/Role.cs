using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EF.Models.Models
{
    public class Role: IdentityRole<Guid>
    {
        public virtual List<UserRole> Users { get; set; }
    }
}
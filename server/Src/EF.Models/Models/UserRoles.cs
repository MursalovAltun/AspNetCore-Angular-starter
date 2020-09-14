using System;

namespace EF.Models.Models
{
    public static class UserRoles
    {
        public class Role
        {
            public string Name { get; }
            public Guid RoleId { get; }

            public Role(Guid roleId, string name)
            {
                RoleId = roleId;
                Name = name;
            }
        }

        public static readonly Role CompanyAdministrator =
            new Role(new Guid("{BB32D7DD-69B4-4518-B085-B89E4D009A7F}"), "CompanyAdministrator");

        public static readonly Role Employee = new Role(new Guid("{527A1B79-D036-4B85-B7AA-C9319FD0A554}"), "Employee");
    }
}
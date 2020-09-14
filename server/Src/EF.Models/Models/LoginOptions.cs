using System;

namespace EF.Models.Models
{
    public class LoginOptions
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string JsonValue { get; set; }
        public virtual User User { get; set; }
    }
}
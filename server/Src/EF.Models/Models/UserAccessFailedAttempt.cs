using System;

namespace EF.Models.Models
{
    public class UserAccessFailedAttempt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        
        public virtual User User { get; set; }
    }
}
using System;
using EF.Models.Interfaces;

namespace EF.Models.Models
{
    public class TodoItem : IUserItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime LastModified { get; set; }

        public virtual User User { get; set; }
    }
}
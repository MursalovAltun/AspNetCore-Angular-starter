using System;
using EF.Models.Models;

namespace EF.Models.Interfaces
{
    public interface IUserItem
    {
        Guid UserId { get; set; }
        User User { get; set; }
    }
}
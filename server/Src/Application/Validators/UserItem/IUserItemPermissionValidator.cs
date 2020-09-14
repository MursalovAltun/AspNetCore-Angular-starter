using System;
using System.Threading.Tasks;
using EF.Models.Interfaces;

namespace Application.Validators.UserItem
{
    public interface IUserItemPermissionValidator<T> where T : class, IUserItem
    {
        Task<bool> HasAccess(Guid entityId);
    }
}
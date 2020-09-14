using System;
using System.Threading.Tasks;

namespace Application.Auth.Webauthn
{
    public interface IWebauthnService
    {
        Task UpdateWebauthnCredential(WebauthnCredentialDto webauthnCredentialDto);

        Task DeleteWebauthnCredential(Guid? webauthnCredentialId);
    }
}
using System;
using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;

namespace Application.Auth.Webauthn
{
    [As(typeof(IWebauthnService))]
    public class WebauthnService : IWebauthnService
    {
        private readonly AppDbContext _context;

        public WebauthnService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateWebauthnCredential(WebauthnCredentialDto webauthnCredentialDto)
        {
            var webauthnCredential = await _context.WebauthnCredentials.FindAsync(webauthnCredentialDto.Id);

            webauthnCredential.Name = webauthnCredentialDto.Name;

            _context.Update(webauthnCredential);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteWebauthnCredential(Guid? webauthnCredentialId)
        {
            var credential = await _context.WebauthnCredentials.FindAsync(webauthnCredentialId);

            _context.Remove(credential);

            await _context.SaveChangesAsync();
        }
    }
}
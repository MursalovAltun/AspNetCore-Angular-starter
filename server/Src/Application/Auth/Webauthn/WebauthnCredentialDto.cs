using System;

namespace Application.Auth.Webauthn
{
    public class WebauthnCredentialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
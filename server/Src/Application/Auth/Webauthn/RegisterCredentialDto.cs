using Fido2NetLib;

namespace Application.Auth.Webauthn
{
    public class RegisterCredentialDto
    {
        public string AuthenticatorName { get; set; }
        public AuthenticatorAttestationRawResponse AttestationRawResponse { get; set; }
    }
}
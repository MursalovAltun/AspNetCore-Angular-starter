using Fido2NetLib;

namespace Application.Auth.Webauthn
{
    public class ValidateLoginDto
    {
        public string UserEmail;
        public AuthenticatorAssertionRawResponse AssertionRawResponse;
    }
}
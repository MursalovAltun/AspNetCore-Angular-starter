using AutoMapper;
using EF.Models.Models;

namespace Application.Auth.Webauthn
{
    public class WebauthnProfile : Profile
    {
        public WebauthnProfile()
        {
            CreateMap<WebauthnCredential, WebauthnCredentialDto>();
        }
    }
}
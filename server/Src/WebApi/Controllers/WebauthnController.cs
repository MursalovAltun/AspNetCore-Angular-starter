using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Auth;
using Application.Auth.Identity;
using Application.Auth.Webauthn;
using Application.Exceptions.BadRequest;
using Application.Providers.CurrentUserProvider;
using AutoMapper;
using EF.Models;
using EF.Models.Models;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSwag.Annotations;
using AuthenticatorTransport = Fido2NetLib.Objects.AuthenticatorTransport;
using PublicKeyCredentialDescriptor = Fido2NetLib.Objects.PublicKeyCredentialDescriptor;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WebauthnController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IAuthenticationResultProvider _authenticationResultProvider;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebauthnService _webauthnService;
        private readonly WebauthnConfiguration _webauthnConfiguration;
        private Fido2 _lib;
        private static IMetadataService _mds;
        private string _origin;

        //https://github.com/abergs/fido2-net-lib/blob/models-refactor/Demo/Controller.cs
        public WebauthnController(
            IUserManager userManager,
            ICurrentUserProvider currentUserProvider,
            IAuthenticationResultProvider authenticationResultProvider,
            AppDbContext context,
            IMapper mapper,
            IOptions<WebauthnConfiguration> webauthnConfiguration,
            IWebauthnService webauthnService
        )
        {
            _userManager = userManager;
            _currentUserProvider = currentUserProvider;
            _authenticationResultProvider = authenticationResultProvider;
            _context = context;
            _mapper = mapper;
            _webauthnService = webauthnService;
            _webauthnConfiguration = webauthnConfiguration.Value;

            SetupFidoLibrary();
        }


        #region Login

        [AllowAnonymous]
        [HttpPost]
        public async Task<AssertionOptions> GetLoginOptions([FromBody] string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new BadRequestException(ErrorCodes.WEBAUTHN_LOGIN_FAILED);

            var existingCredentials = GetWebauthnCredentialDescriptors(user);
            if (existingCredentials == null || !existingCredentials.GetEnumerator().MoveNext())
                throw new BadRequestException(ErrorCodes.WEBAUTHN_LOGIN_FAILED);

            var exts = new AuthenticationExtensionsClientInputs()
            {
                AppID = _origin,
                SimpleTransactionAuthorization = "FIDO",
                GenericTransactionAuthorization = new TxAuthGenericArg
                    {ContentType = "text/plain", Content = new byte[] {0x46, 0x49, 0x44, 0x4F}},
                UserVerificationIndex = true,
                Location = true,
                UserVerificationMethod = true
            };

            var uv = UserVerificationRequirement.Required;
            var options = _lib.GetAssertionOptions(
                existingCredentials,
                uv,
                exts
            );

            await SetLoginOptions(user, options);

            return options;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResult> ValidateLogin(
            [FromBody] ValidateLoginDto clientResponse)
        {
            var user = await _userManager.FindByEmailAsync(clientResponse.UserEmail);

            var options = GetLoginOptions(user);

            bool MatchingDescriptorId(WebauthnCredential p) =>
                p.Descriptor.Id.SequenceEqual(clientResponse.AssertionRawResponse.Id);

            var creds = user.WebauthnCredentials.FirstOrDefault(MatchingDescriptorId);

            if (creds == null)
            {
                throw new BadRequestException(ErrorCodes.WEBAUTHN_LOGIN_FAILED);
            }

            var storedCounter = creds.SignatureCounter;

            var res = await _lib
                .MakeAssertionAsync(clientResponse.AssertionRawResponse, options, creds.PublicKey, storedCounter,
                    async args => true);

            var authenticationResult = _authenticationResultProvider.Get(user);

            creds.SignatureCounter = res.Counter;

            var webauthnCredential = user.WebauthnCredentials.First(MatchingDescriptorId);
            webauthnCredential.SignatureCounter = res.Counter;

            _context.Update(webauthnCredential);

            await _context.SaveChangesAsync();

            return authenticationResult;
        }

        #endregion

        #region Register

        [HttpPost]
        [SwaggerResponse(typeof(CredentialCreateOptions))]
        public async Task<ContentResult> GetRegisterOptions()
        {
            var user = await GetCurrentUser();

            var fidoUser = new Fido2User
                {DisplayName = user.Email, Id = user.Id.ToByteArray(), Name = user.UserName};

            var options = _lib.RequestNewCredential(fidoUser, GetWebauthnCredentialDescriptors(user),
                AuthenticatorSelection.Default,
                AttestationConveyancePreference.Direct);

            await SetRegisterOptions(user, options);

            return new JsonStringResult(user.RegisterOptions.JsonValue);
        }

        [HttpPost]
        public async Task<WebauthnCredentialDto> ValidateRegister(
            [FromBody] RegisterCredentialDto registerCredentialData)
        {
            var user = await GetCurrentUser();

            var options = GetRegisterOptions(user);

            IsCredentialIdUniqueToUserAsyncDelegate callback = async (IsCredentialIdUniqueToUserParams args) => true;

            var success =
                await _lib.MakeNewCredentialAsync(registerCredentialData.AttestationRawResponse, options, callback);


            var publicKeyCredentialDescriptor = new PublicKeyCredentialDescriptor(success.Result.CredentialId);
            var credentialDescriptor = new EF.Models.Models.PublicKeyCredentialDescriptor
            {
                Id = publicKeyCredentialDescriptor.Id,
                Type = PublicKeyCredentialType.PublicKey,
                Transports = (publicKeyCredentialDescriptor.Transports ?? new AuthenticatorTransport[0])
                    .Select(t => new EF.Models.Models.AuthenticatorTransport
                    {
                        Transport = t
                    })
                    .ToList()
            };

            var webauthnCredential = new WebauthnCredential
            {
                Name = registerCredentialData.AuthenticatorName,
                PublicKey = success.Result.PublicKey,
                Descriptor = credentialDescriptor,
                CreatedOn = DateTime.UtcNow,
                SignatureCounter = success.Result.Counter
            };

            user.WebauthnCredentials.Add(webauthnCredential);

            _context.Update(user);

            await _context.SaveChangesAsync();

            return _mapper.Map<WebauthnCredentialDto>(webauthnCredential);
        }

        #endregion

        #region admin

        [HttpGet]
        public async Task<IEnumerable<WebauthnCredentialDto>> GetUserCredentials()
        {
            var user = await GetCurrentUser();
            var credentials = user.WebauthnCredentials;
            return _mapper.Map<IEnumerable<WebauthnCredentialDto>>(credentials);
        }

        [HttpDelete]
        public async Task RemoveUserCredential([FromQuery] WebauthnCredentialIdRequest request)
        {
            await _webauthnService.DeleteWebauthnCredential(request.WebauthnCredentialId);
        }

        [HttpPost]
        public async Task EditUserCredential([FromBody] WebauthnCredentialDto credential)
        {
            await _webauthnService.UpdateWebauthnCredential(credential);
        }

        #endregion

        private void SetupFidoLibrary()
        {
            var mdsAccessKey = _webauthnConfiguration.MDSAccessKey;
            _mds = string.IsNullOrEmpty(mdsAccessKey)
                ? null
                : MDSMetadata.Instance(mdsAccessKey, _webauthnConfiguration.MDSCacheDirPath);
            if (null != _mds)
            {
                if (false == _mds.IsInitialized())
                    _mds.Initialize().Wait();
            }

            _origin = _webauthnConfiguration.Origin;

            _lib = new Fido2(new Fido2Configuration()
            {
                ServerDomain = _webauthnConfiguration.ServerDomain,
                ServerName = _webauthnConfiguration.ServerName,
                Origin = _origin,
                // Only create and use Metadataservice if we have an acesskey
                MetadataService = _mds
            });
        }

        private async Task SetLoginOptions(User user, AssertionOptions options)
        {
            user.LoginOptions ??= new LoginOptions();

            user.LoginOptions.JsonValue = options.ToJson();

            await _context.SaveChangesAsync();
        }

        private AssertionOptions GetLoginOptions(User user)
        {
            return AssertionOptions.FromJson(user.LoginOptions.JsonValue);
        }

        private async Task<User> GetCurrentUser()
        {
            var user = await _currentUserProvider.GetUserAsync();
            if (user == null)
                throw new BadRequestException(ErrorCodes.WEBAUTHN_LOGIN_FAILED);
            return user;
        }

        private async Task SetRegisterOptions(User user, CredentialCreateOptions options)
        {
            user.RegisterOptions ??= new RegisterOptions();

            user.RegisterOptions.JsonValue = options.ToJson();

            await _context.SaveChangesAsync();
        }

        private CredentialCreateOptions GetRegisterOptions(User user)
        {
            return CredentialCreateOptions.FromJson(user.RegisterOptions.JsonValue);
        }

        private List<PublicKeyCredentialDescriptor> GetWebauthnCredentialDescriptors(User user)
        {
            return user.WebauthnCredentials
                .Select(credential => credential.Descriptor)
                .Select(descriptor => new PublicKeyCredentialDescriptor
                {
                    Id = descriptor.Id,
                    Transports = descriptor.Transports.Select(t => t.Transport).ToArray(),
                    Type = descriptor.Type
                })
                .ToList();
        }
    }

    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }
}
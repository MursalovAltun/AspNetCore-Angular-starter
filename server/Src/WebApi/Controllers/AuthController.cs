using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Auth;
using Application.Auth.Identity;
using Application.Components.Captcha;
using Application.Exceptions.BadRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUserAccessFailedService _userAccessFailedService;
        private readonly ICaptchaValidationService _captchaValidationService;
        private readonly IAuthenticationResultProvider _authenticationResultProvider;

        public AuthController(IUserManager userManager,
            IUserAccessFailedService userAccessFailedService,
            ICaptchaValidationService captchaValidationService,
            IAuthenticationResultProvider authenticationResultProvider)
        {
            _userManager = userManager;
            _userAccessFailedService = userAccessFailedService;
            _captchaValidationService = captchaValidationService;
            _authenticationResultProvider = authenticationResultProvider;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new BadRequestException(ErrorCodes.LOGIN_FAILED);

            // var captchaRequiredUntil = await _userAccessFailedService.IsCaptchaRequired(user);
            // if (captchaRequiredUntil != null)
            // {
            //     var isTokenValid = await _captchaValidationService.IsValidAsync(request.CaptchaToken);
            //     if (!isTokenValid)
            //         throw new BadRequestException(ErrorCodes.LOGIN_FAILED, new AuthenticateUnauthorizedResult
            //         {
            //             CaptchaRequiredUntil = captchaRequiredUntil,
            //         });
            // }

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (isValid) return _authenticationResultProvider.Get(user);

            await _userAccessFailedService.RegisterFailedAttempt(user);

            throw new BadRequestException(ErrorCodes.LOGIN_FAILED, new AuthenticateUnauthorizedResult
            {
                CaptchaRequiredUntil = await _userAccessFailedService.IsCaptchaRequired(user)
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<AuthMethods> GetAuthMethods([FromQuery] string email)
        {
            var authMethods = new AuthMethods
            {
                Email = email,
                Password = true,
            };

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return authMethods;
            }

            authMethods.WebAuthn = user.WebauthnCredentials.Any();

            return authMethods;
        }
    }

    public class AuthMethods
    {
        public string Email { get; set; }
        public bool Password { get; set; }
        public bool WebAuthn { get; set; }
    }
}
using Application.Components.Captcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class AppSettingsController : Controller
    {
        private readonly CaptchaOptions _captchaOptions;

        public AppSettingsController(IOptions<CaptchaOptions> captchaOptions)
        {
            _captchaOptions = captchaOptions.Value;
        }

        [HttpGet]
        public AppSettingsDto Get()
        {
            return new AppSettingsDto
            {
                CaptchaClientKey = _captchaOptions.ClientKey
            };
        }
    }

    public class AppSettingsDto
    {
        public string CaptchaClientKey { get; set; }
    }
}
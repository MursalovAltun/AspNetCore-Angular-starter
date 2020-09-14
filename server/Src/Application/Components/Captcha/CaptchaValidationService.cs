using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Components.Captcha
{
    public class CaptchaValidationService : ICaptchaValidationService
    {
        private readonly CaptchaOptions _options;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;

        public CaptchaValidationService(
            IOptions<CaptchaOptions> options,
            IHttpContextAccessor contextAccessor,
            HttpClient httpClient)
        {
            _contextAccessor = contextAccessor;
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<bool> IsValidAsync(string validationToken)
        {
            if (string.IsNullOrEmpty(validationToken))
                return false;

            var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _options.Secret),
                new KeyValuePair<string, string>("response", validationToken),
                new KeyValuePair<string, string>("remoteip", ip)
            });

            using var res = await _httpClient.PostAsync(_options.VerificationUrl, formContent);
            using var content = res.Content;
            var data = await content.ReadAsStringAsync();

            if (data == null) return false;

            var response = (JObject) JsonConvert.DeserializeObject(data);
            var success = response["success"];

            return success?.Value<bool>() ?? false;
        }
    }
}
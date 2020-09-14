using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Auth
{
    public class JwtTokenOptions
    {
        public string Key { get; set; }
        public int ExpireDays { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
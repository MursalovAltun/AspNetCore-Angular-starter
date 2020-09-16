using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Auth
{
    public class JwtTokenOptions
    {
        [Required] public string Key { get; set; }
        [Required] public int ExpireDays { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
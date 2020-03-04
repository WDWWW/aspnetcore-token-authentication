using System.Security.Claims;
using System.Threading.Tasks;

namespace Wd3w.TokenAuthentication.Sample
{
    public class CustomTokenService : ITokenAuthService
    {
        public Task<bool> IsValidateAsync(string token)
        {
            return Task.FromResult(true);
        }

        public Task<ClaimsPrincipal> GetPrincipalAsync(string token)
        {
            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "test@user.com"),
            })));
        }
    }
}
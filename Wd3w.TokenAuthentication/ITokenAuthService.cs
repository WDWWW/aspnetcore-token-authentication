using System.Security.Claims;
using System.Threading.Tasks;

namespace Wd3w.TokenAuthentication
{
    public interface ITokenAuthService
    {
        Task<bool> IsValidateAsync(string token);

        Task<ClaimsPrincipal> GetPrincipalAsync(string token);
    }
}
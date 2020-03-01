using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Wd3w.TokenAuthentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationHandlerOptions> 
    {
        private readonly ITokenAuthService _authService;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationHandlerOptions> options,
            ITokenAuthService authService,
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authenticateResult = await HandleAuthenticateOnceSafeAsync();

            Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            if (authenticateResult.Failure == null)
                return;

            var failureMessage = authenticateResult.Failure.Message;

            var message = authenticateResult.Failure is AuthenticationFailException
                ? failureMessage
                : $@"error=""{failureMessage}""";

            if (authenticateResult.Failure is AuthenticationFailException)
                Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"{Options.Scheme} realm=""{Options.Realm}"", {message}");
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var value))
                return AuthenticateResult.NoResult();

            var authPair = value.ToString().Split(" ").ToList();

            var scheme = Options.Scheme;
            if (authPair.Count != 2)
                return Fail("invalid_format", $"Authorization must be formatted as '{scheme} <token>'");

            if (authPair[0] != Scheme.Name)
                return Fail("invalid_scheme", $"Scheme must be {scheme}");

            if (authPair[1].Length != Options.TokenLength)
                return Fail("invalid_token_length", $"Access Token`s length must be {Options.TokenLength}");
            
            var accessToken = authPair[1];
            if (!await _authService.IsValidateAsync(accessToken))
                return Fail("invalid_token", $"{scheme} Token({accessToken}) is invalid");

            var claimsPrincipal = await _authService.GetPrincipalAsync(accessToken);
            var wrapPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimsPrincipal.Claims, Options.AuthenticationType ?? scheme));
            return AuthenticateResult.Success(new AuthenticationTicket(wrapPrincipal, Scheme.Name));
        }

        private static AuthenticateResult Fail(string error, string description)
        {
            return AuthenticateResult.Fail(new AuthenticationFailException(error, description));
        }
    }
}
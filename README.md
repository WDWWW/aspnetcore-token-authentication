# Wd3w.TokenAuthentication
> ASP.NET Core extension for configure custom token authentication

## Getting Start 

### 1. Implement your own `CustomTokenAuthService`  

```csharp
public class CustomTokenAuthService : ITokenAuthService
{
    private CustomDb _db { get; set; }

    public CustomTokenAuthService(CustomDb db)
    {
        _db = db;
    }

    public async Task<bool> IsValidateAsync(string token)
    {
        return await _db.AccessTokens.AnyAsync(accessToken => accessToken.Key == token);
    }

    public async Task<ClaimsPrincipal> GetPrincipalAsync(string token)
    {
        // Do create your own custom claims pricipal object and return them;
        return new ...
    }
}
```

### 2. Add custom token scheme to authentication builder using `AddTokenAuthenticationScheme<TService>` 

```csharp
services.AddAuthentication("Bearer")
    .AddTokenAuthenticationScheme<CustomTokenAuthService>("Bearer", new TokenAuthenticationConfiguration
    {
        Realm = "www.example.com/sign-in",
        TokenLength = 21,
        // AuthenticationType = "Bearer" - this value is optional, default is from scheme parameter value.
    });
```
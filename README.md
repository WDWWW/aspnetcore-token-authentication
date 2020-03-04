# Wd3w.TokenAuthentication
> ASP.NET Core extension for configure custom token authentication

![Build&Test](https://github.com/WDWWW/aspnetcore-token-authentication/workflows/Build&Test/badge.svg?branch=master)

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


## Versioning

This package versioning strategy is following AspNetCore package version. When there are new release about AspNetCore, This package also will be update.

Only minor version is mismatched AspNetCore, that will use for fixing bugs and some changes.

## License

MIT License

Copyright (c) 2020 WDWWW

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

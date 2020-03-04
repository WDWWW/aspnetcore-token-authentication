# Wd3w.TokenAuthentication
> ASP.NET Core extension for configure custom token authentication

![Build&Test](https://github.com/WDWWW/aspnetcore-token-authentication/workflows/Build&Test/badge.svg?branch=master)
[![Nuget](https://img.shields.io/nuget/v/Wd3w.AspNetCore.TokenAuthentication)](https://www.nuget.org/packages/Wd3w.AspNetCore.TokenAuthentication/)

## Getting Start 

#### 1. Installation package to your project.

```
dotnet add package Wd3w.AspNetCore.TokenAuthentication
```

#### 2. Implement your own `CustomTokenAuthService` 

Whatever your authentication infrastructure is,  just implement this interface and register it as a service.

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

#### 3. Add custom token scheme to authentication builder using `AddTokenAuthenticationScheme<TService>` 

`Realm`, `TokenLength`, These property is required property for working authentication handler. 

```csharp
services.AddAuthentication("Bearer")
    .AddTokenAuthenticationScheme<CustomTokenAuthService>("Bearer", new TokenAuthenticationConfiguration
    {
        Realm = "www.example.com/sign-in",
        TokenLength = 21,
        // AuthenticationType = "Bearer" - this value is optional, default is from scheme parameter value.
    });
```

#### 4. Attach `AuthorizeAttribute` to your controller or action methods.

```c#
[ApiController("[controller]")]
public class SomeController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public Task<ActionResult> GetSomethingAsync()
    {
        return Task.FromResult(Ok());
    }
}
```

## Features

#### Custom Validation Fail Message 

If you need to provide custom authentication token validation message, Just throw `AuthenticationFailException` on `ITokenAuthService.IsValidateAsync`

```c#
public class CustomTokenAuthService : ITokenAuthService
{
    public async Task<bool> IsValidateAsync(string token)
    {
        throw new AuthenticationFailException("invalid_format", "access token couldn't have any special characters.");
    }
}
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

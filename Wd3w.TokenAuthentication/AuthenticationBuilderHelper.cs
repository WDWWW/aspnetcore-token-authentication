using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Wd3w.TokenAuthentication
{
    public static class AuthenticationBuilderHelper
    {
        /// <summary>
        ///     Add custom token authentication scheme.
        /// </summary>
        /// <param name="builder">The authentication builder</param>
        /// <param name="scheme">The scheme of custom token</param>
        /// <param name="config">The configuration props for registering custom scheme.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void AddTokenAuthenticationScheme(this AuthenticationBuilder builder, [NotNull] string scheme, [NotNull] TokenAuthenticationConfiguration config)
        {
            if (config == null)
                throw new ArgumentException($"{nameof(config)} parameter must not be null.");
            
            if (config.TokenLength <= 0)
                throw new ArgumentException($"{nameof(config.TokenLength)} must be greater than zero.");
            
            if (config.Realm == null)
                throw new ArgumentException($"{nameof(config.Realm)} Property must not be null.");

            builder.AddScheme<TokenAuthenticationHandlerOptions, TokenAuthenticationHandler>(scheme, options =>
            {
                options.Scheme = scheme;
                options.Realm = config.Realm;
                options.AuthenticationType = config.AuthenticationType;
                options.TokenLength = config.TokenLength;
            });
        }
        
        /// <summary>
        ///     Add custom token authentication scheme.
        /// </summary>
        /// <param name="builder">The authentication builder</param>
        /// <param name="scheme">The scheme of custom token</param>
        /// <param name="config">The configuration props for registering custom scheme.</param>
        /// <typeparam name="TAuthServiceImplementation">ITokenAuthService implementation type.</typeparam>
        public static void AddTokenAuthenticationScheme<TAuthServiceImplementation>(this AuthenticationBuilder builder,
            [NotNull] string scheme, [NotNull] TokenAuthenticationConfiguration config)
            where TAuthServiceImplementation : class, ITokenAuthService
        {
            builder.Services.AddScoped<ITokenAuthService, TAuthServiceImplementation>();
            AddTokenAuthenticationScheme(builder, scheme, config);
        }
        
        /// <summary>
        ///     Add custom token authentication scheme.
        /// </summary>
        /// <param name="builder">The authentication builder</param>
        /// <param name="scheme">The scheme of custom token</param>
        /// <param name="config">The configuration props for registering custom scheme.</param>
        /// <param name="serviceLifetime">Service Lifetime of TAuthServiceImplementation</param>
        /// <typeparam name="TAuthServiceImplementation">ITokenAuthService implementation type.</typeparam>
        public static void AddTokenAuthenticationScheme<TAuthServiceImplementation>(this AuthenticationBuilder builder,
            [NotNull] string scheme, [NotNull] TokenAuthenticationConfiguration config, ServiceLifetime serviceLifetime)
            where TAuthServiceImplementation : class, ITokenAuthService
        {
            builder.Services.Add(new ServiceDescriptor(typeof(ITokenAuthService), typeof(TAuthServiceImplementation), serviceLifetime));
            AddTokenAuthenticationScheme(builder, scheme, config);
        }
    }
}
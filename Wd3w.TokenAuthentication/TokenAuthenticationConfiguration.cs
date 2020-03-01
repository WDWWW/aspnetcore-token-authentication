namespace Wd3w.TokenAuthentication
{
    public class TokenAuthenticationConfiguration
    {
        /// <summary>
        ///     The value is used by ClaimsIdentity.AuthenticationType. Scheme property value will be used if this value is null.
        /// </summary>
        public string AuthenticationType { get; set; }
        
        public string Realm { get; set; }

        /// <summary>
        ///     Length of custom token.
        /// </summary>
        public int TokenLength { get; set; }
    }
}
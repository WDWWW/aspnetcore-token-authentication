
using System;

namespace Wd3w.TokenAuthentication
{
    public class AuthenticationFailException : Exception
    {
        public string Error { get; }

        public string Description { get; }

        public AuthenticationFailException(string error, string description)
        {
            Error = error;
            Description = description;
        }
    }
}
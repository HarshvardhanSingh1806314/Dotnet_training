using System;

namespace CustomExceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException(string message) : base(message) { }
    }
}

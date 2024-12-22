using System;

namespace CustomExceptions
{
    public class InsufficientSeatsException : Exception
    {
        public InsufficientSeatsException(string message) : base(message) { }
    }
}

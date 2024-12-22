using System;

namespace CustomExceptions
{
    public class NullValueException : Exception
    {
        public NullValueException(string message) : base(message)
        {

        }
    }
}

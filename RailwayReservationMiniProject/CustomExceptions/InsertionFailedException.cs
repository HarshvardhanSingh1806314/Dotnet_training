using System;

namespace CustomExceptions
{
    public class InsertionFailedException : Exception
    {
        public InsertionFailedException(string message) : base(message)
        {

        }
    }
}

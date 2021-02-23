using System;

namespace budget4home.Helpers
{
    public class ForbidException : Exception
    {
        public ForbidException()
        {
        }

        public ForbidException(string message)
            : base(message)
        {
        }

        public ForbidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
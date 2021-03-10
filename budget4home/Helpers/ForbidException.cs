using System;

namespace budget4home.Helpers
{
    public class DbException : Exception
    {
        public DbException()
        {
        }

        public DbException(string message)
            : base(message)
        {
        }

        public DbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
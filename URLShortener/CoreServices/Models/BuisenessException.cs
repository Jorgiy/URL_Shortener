using System;

namespace CoreServices.Models
{
    public class BuisenessException : Exception
    {
        public BuisenessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
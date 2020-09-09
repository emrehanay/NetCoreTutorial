using System;

namespace NetCoreTutorial.Helpers
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}
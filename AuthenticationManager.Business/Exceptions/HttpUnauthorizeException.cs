using System;

namespace AuthenticationManager.Business.Exceptions
{
    public class HttpUnauthorizeException : HttpStatusCodeException
    {
        private const int UNAUTHORIZE = 401;

        public HttpUnauthorizeException(int statusCode = UNAUTHORIZE) : base(statusCode)
        {
        }

        public HttpUnauthorizeException(string message, int statusCode = UNAUTHORIZE) : base(statusCode, message)
        {
        }

        public HttpUnauthorizeException(Exception inner, int statusCode = UNAUTHORIZE) : base(statusCode, inner)
        {
        }
    }

}

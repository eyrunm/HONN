using System;
using System.Runtime.Serialization;

namespace LibraryAPI.Repositories
{
    [Serializable]
    public class RatingException : Exception
    {
        public RatingException()
        {
        }
        public RatingException(string message) : base(message)
        {
        }

        public RatingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RatingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
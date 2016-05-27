using System;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class ReadFileException : Exception
    {
        public ReadFileException()
        {
        }

        public ReadFileException(string message) : base(message)
        {
        }

        public ReadFileException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ReadFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
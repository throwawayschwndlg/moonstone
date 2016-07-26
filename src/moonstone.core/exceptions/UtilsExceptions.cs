using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class RequestJsonException : Exception
    {
        public RequestJsonException()
        {
        }

        public RequestJsonException(string message) : base(message)
        {
        }

        public RequestJsonException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RequestJsonException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
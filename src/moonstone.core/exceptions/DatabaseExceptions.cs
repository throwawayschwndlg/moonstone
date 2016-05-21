using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.exceptions
{

    [Serializable]
    public class InitializeSqlConnectionException : Exception
    {
        public InitializeSqlConnectionException() { }
        public InitializeSqlConnectionException(string message) : base(message) { }
        public InitializeSqlConnectionException(string message, Exception inner) : base(message, inner) { }
        protected InitializeSqlConnectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class CreateDatabaseException : Exception
    {
        public CreateDatabaseException() { }
        public CreateDatabaseException(string message) : base(message) { }
        public CreateDatabaseException(string message, Exception inner) : base(message, inner) { }
        protected CreateDatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class DropDatabaseException : Exception
    {
        public DropDatabaseException() { }
        public DropDatabaseException(string message) : base(message) { }
        public DropDatabaseException(string message, Exception inner) : base(message, inner) { }
        protected DropDatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class ReadDatabaseVerionException : Exception
    {
        public ReadDatabaseVerionException() { }
        public ReadDatabaseVerionException(string message) : base(message) { }
        public ReadDatabaseVerionException(string message, Exception inner) : base(message, inner) { }
        protected ReadDatabaseVerionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

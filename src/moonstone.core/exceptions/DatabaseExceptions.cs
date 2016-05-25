using System;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class InitializeSqlConnectionException : Exception
    {
        public InitializeSqlConnectionException()
        {
        }

        public InitializeSqlConnectionException(string message) : base(message)
        {
        }

        public InitializeSqlConnectionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InitializeSqlConnectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CreateDatabaseException : Exception
    {
        public CreateDatabaseException()
        {
        }

        public CreateDatabaseException(string message) : base(message)
        {
        }

        public CreateDatabaseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateDatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DropDatabaseException : Exception
    {
        public DropDatabaseException()
        {
        }

        public DropDatabaseException(string message) : base(message)
        {
        }

        public DropDatabaseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DropDatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ReadDatabaseVerionException : Exception
    {
        public ReadDatabaseVerionException()
        {
        }

        public ReadDatabaseVerionException(string message) : base(message)
        {
        }

        public ReadDatabaseVerionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ReadDatabaseVerionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CreateVersionTableException : Exception
    {
        public CreateVersionTableException()
        {
        }

        public CreateVersionTableException(string message) : base(message)
        {
        }

        public CreateVersionTableException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateVersionTableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class VersionTableAlreadyExistsException : Exception
    {
        public VersionTableAlreadyExistsException()
        {
        }

        public VersionTableAlreadyExistsException(string message) : base(message)
        {
        }

        public VersionTableAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected VersionTableAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class RetreiveInstalledVersionException : Exception
    {
        public RetreiveInstalledVersionException()
        {
        }

        public RetreiveInstalledVersionException(string message) : base(message)
        {
        }

        public RetreiveInstalledVersionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RetreiveInstalledVersionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InitializeDatabaseException : Exception
    {
        public InitializeDatabaseException()
        {
        }

        public InitializeDatabaseException(string message) : base(message)
        {
        }

        public InitializeDatabaseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InitializeDatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class AddInstalledVersionException : Exception
    {
        public AddInstalledVersionException()
        {
        }

        public AddInstalledVersionException(string message) : base(message)
        {
        }

        public AddInstalledVersionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AddInstalledVersionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class LowerOrEqualVersionException : Exception
    {
        public LowerOrEqualVersionException()
        {
        }

        public LowerOrEqualVersionException(string message) : base(message)
        {
        }

        public LowerOrEqualVersionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LowerOrEqualVersionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ExecuteScriptException : Exception
    {
        public ExecuteScriptException()
        {
        }

        public ExecuteScriptException(string message) : base(message)
        {
        }

        public ExecuteScriptException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExecuteScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class MetaQueryException : Exception
    {
        public MetaQueryException()
        {
        }

        public MetaQueryException(string message) : base(message)
        {
        }

        public MetaQueryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MetaQueryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DropTableException : Exception
    {
        public DropTableException()
        {
        }

        public DropTableException(string message) : base(message)
        {
        }

        public DropTableException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DropTableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
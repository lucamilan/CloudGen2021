using System;
using System.Runtime.Serialization;

namespace Dapr.Cqrs.Common.Exceptions
{
    [Serializable]
    public class SqlServerNotAvailableException : Exception
    {
        public SqlServerNotAvailableException()
        {
        }

        public SqlServerNotAvailableException(string message) : base(message)
        {
        }

        public SqlServerNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SqlServerNotAvailableException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
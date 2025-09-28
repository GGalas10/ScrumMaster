using System.Runtime.Serialization;

namespace ScrumMaster.Project.Infrastructure.CustomExceptions
{
    public class RoleException : Exception
    {
        public RoleException()
        {
        }

        public RoleException(string? message) : base(message)
        {
        }

        public RoleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

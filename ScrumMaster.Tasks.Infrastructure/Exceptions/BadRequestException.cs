namespace ScrumMaster.Tasks.Infrastructure.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}

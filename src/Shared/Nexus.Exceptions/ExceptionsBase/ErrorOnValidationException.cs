namespace Nexus.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : NexusException
    {
        public IList<string> ErrorsMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorsMessages) : base(string.Empty)
        {
            ErrorsMessages = errorsMessages;
        }
    }
}

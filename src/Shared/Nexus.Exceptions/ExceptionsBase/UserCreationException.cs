namespace Nexus.Exceptions.ExceptionsBase
{
    public class UserCreationException : NexusException
    {
        public UserCreationException() : base(ResourceMessagesException.USER_CREATION_ERROR)
        {
        }
    }
}

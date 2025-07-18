namespace Nexus.Exceptions.ExceptionsBase
{
    public class SeedDataException : NexusException
    {
        public SeedDataException() : base(ResourceMessagesException.SEED_DATA_ERROR)
        {
        }
    }
}

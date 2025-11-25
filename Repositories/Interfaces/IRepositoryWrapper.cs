namespace CommunityEvents.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IEventRepository EventRepository { get; }
        IRegistrationRepository RegistrationRepository { get; }

        void Save();
    }
}

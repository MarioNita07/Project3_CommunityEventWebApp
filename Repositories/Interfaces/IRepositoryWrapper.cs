namespace CommunityEvents.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IEventRepository EventRepository { get; }
        IRegistrationRepository RegistrationRepository { get; }
        IReviewRepository ReviewRepository { get; }
        INotificationRepository NotificationRepository { get; }

        void Save();
    }
}

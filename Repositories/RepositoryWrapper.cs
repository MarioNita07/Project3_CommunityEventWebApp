using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;

namespace CommunityEvents.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private CommunityEventContext _communityEventContext;
        private IEventRepository? _eventRepository;
        private IRegistrationRepository? _registrationRepository;

        public IEventRepository EventRepository
        {
            get
            {
                if (_eventRepository == null)
                {
                    _eventRepository = new EventRepository(_communityEventContext);
                }
                return _eventRepository;
            }
        }

        public IRegistrationRepository RegistrationRepository
        {
            get
            {
                if (_registrationRepository == null)
                {
                    _registrationRepository = new RegistrationRepository(_communityEventContext);
                }
                return _registrationRepository;
            }
        }

        public RepositoryWrapper(CommunityEventContext communityEventContext)
        {
            _communityEventContext = communityEventContext;
        }

        public void Save()
        {
            _communityEventContext.SaveChanges();
        }
    }
}

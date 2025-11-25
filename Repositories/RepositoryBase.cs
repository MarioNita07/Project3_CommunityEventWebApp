using CommunityEvents.Repositories.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Models;
namespace CommunityEvents.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected CommunityEventContext CommunityEventContext { get; set; }

        public RepositoryBase(CommunityEventContext communityEventContext)
        {
            this.CommunityEventContext = communityEventContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.CommunityEventContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.CommunityEventContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.CommunityEventContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.CommunityEventContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.CommunityEventContext.Set<T>().Remove(entity);
        }
    }
}

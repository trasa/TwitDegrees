using System.Collections.Generic;
using System.Linq;
using Blackfin.Core.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace TwitDegrees.Presentation.Core.Repositories
{
    public interface IRepository<T>
    {
        T Get(int id);

        IEnumerable<T> GetAll();

        void Delete(T entity);

        T Save(T entity);

        List<T> FindAllLike(T instance, params string[] propertiesToExclude);

        // querying because we don't want to expose ICriteria to the outside world.
        IList<T> QueryList(DetachedCriteria query);
        TResult QueryUniqueResult<TResult>(DetachedCriteria query);
    }

    public class Repository<T> : IRepository<T>
    {
        public Repository(NHibernateSession session)
        {
            Session = session.Current;
        }

        protected virtual ISession Session { get; private set; }

        public T Get(int id)
        {
            return Session.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Session
                .CreateCriteria(typeof(T))
                .List<T>();
        }

        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public virtual T Save(T entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public List<T> FindAllLike(T instance, params string[] propertiesToExclude)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            Example example = Example.Create(instance);
            propertiesToExclude.ToList().ForEach(prop => example.ExcludeProperty(prop));
            criteria.Add(example);
            return criteria.List<T>().ToList();
        }

        // note: exposes NHibernate internals... might need to make it public.
        // but i really don't want to.   Linq To NHibernate, where are you in our time of need.
        protected ICriteria Criteria(DetachedCriteria query)
        {
            return query.GetExecutableCriteria(Session);
        }

        /// <summary>
        /// Execute an arbitary Criteria on the current session
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IList<T> QueryList(DetachedCriteria query)
        {
            return Criteria(query).List<T>();
        }

        public TResult QueryUniqueResult<TResult>(DetachedCriteria query)
        {
            return Criteria(query).UniqueResult<TResult>();
        }
    }
}

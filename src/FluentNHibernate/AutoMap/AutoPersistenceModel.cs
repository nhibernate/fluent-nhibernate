using FluentNHibernate;

namespace FluentNHibernate.AutoMap
{
    public class AutoPersistanceModel : PersistenceModel
    {
        public AutoPersistanceModel AutoMap<T>()
        {
            var autoMap = new AutoMapper();
            addMapping(autoMap.Map<T>());
            return this;
        }

    }
}
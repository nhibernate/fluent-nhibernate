namespace FluentNHibernate.MappingModel
{
    public abstract class MappingBase<T> : IMappingBase where T : class, new()
    {
        protected readonly T _hbm;

        protected MappingBase()
        {
            _hbm = new T();
        }

        public T Hbm
        {
            get { return _hbm; }
        }

        object IMappingBase.Hbm
        {
            get { return Hbm; }
        }

    }
}
using System;

namespace FluentNHibernate.Conventions.Instances
{
    /// <inheritdoc cref="IGeneratedInstance"/>
    public class GeneratedInstance : IGeneratedInstance
    {
        private readonly Action<string> setter;

        public GeneratedInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        /// <inheritdoc />
        public void Never()
        {
            setter("never");
        }

        /// <inheritdoc />
        public void Insert()
        {
            setter("insert");
        }

        /// <inheritdoc />
        public void Always()
        {
            setter("always");
        }
    }
}
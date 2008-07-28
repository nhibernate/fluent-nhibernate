using System.IO;
using System.Reflection;

namespace FluentNHibernate.Generation
{
    public interface IProperty
    {
        PropertyInfo Property { set; }
        void WritePropertiesForDomainClassFixture(TextWriter writer);
        void WritePropertiesForListFixture(TextWriter writer);
    }

    public class NulloProperty : IProperty
    {
        #region IProperty Members

        public void WritePropertiesForDomainClassFixture(TextWriter writer)
        {
        }

        public void WritePropertiesForListFixture(TextWriter writer)
        {
        }

        public PropertyInfo Property
        {
            set
            {
                // no-op; 
            }
        }

        #endregion
    }
}
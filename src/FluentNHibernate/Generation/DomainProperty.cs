using System.IO;
using System.Reflection;

namespace ShadeTree.DomainModel.Generation
{
    public class DomainProperty : IProperty
    {
        private PropertyInfo _property;

        public DomainProperty()
        {
        }

        #region IProperty Members

        public void WritePropertiesForDomainClassFixture(TextWriter writer)
        {
            if (!_property.CanWrite)
            {
                return;
            }

            writer.WriteLine();
            writer.WriteLine("\t\t[Example(\"|{0} is|[value]|\")]", _property.Name);
            writer.WriteLine("\t\tpublic void {0}Is(string propertyValue)", _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSubject.{0} = DomainObjectFinder.Find<{1}>(propertyValue);", _property.Name,
                             _property.PropertyType.FullName);
            writer.WriteLine("\t\t}");
        }


        public void WritePropertiesForListFixture(TextWriter writer)
        {
            if (!_property.CanWrite)
            {
                return;
            }

            writer.WriteLine();
            writer.WriteLine("\t\tpublic string {0}", _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tsubject.{0} = DomainObjectFinder.Find<{1}>(value);", _property.Name,
                             _property.PropertyType.FullName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        public PropertyInfo Property
        {
            set { _property = value; }
        }

        #endregion
    }
}
using System.IO;
using System.Reflection;

namespace FluentNHibernate.Generation
{
    public class SimpleProperty : IProperty
    {
        private PropertyInfo _property;

        public SimpleProperty()
        {
        }




        public void WritePropertiesForDomainClassFixture(TextWriter writer)
        {
            if (_property.CanWrite)
            {
                writer.WriteLine();
                writer.WriteLine("\t\t[Example(\"|{0} is|[value]|\")]", _property.Name);
                writer.WriteLine("\t\tpublic void {0}Is({1} propertyValue)", _property.Name, _property.PropertyType.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tSubject.{0} = propertyValue;", _property.Name);
                writer.WriteLine("\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t[Example(\"|Check|{0} is|[value]|\")]", _property.Name);
            writer.WriteLine("\t\tpublic {0} {1}Is()", _property.PropertyType, _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn Subject.{0};", _property.Name);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        public void WritePropertiesForListFixture(TextWriter writer)
        {
            if (!_property.CanWrite)
            {
                return;
            }


            writer.WriteLine();
            writer.WriteLine("\t\tpublic {0} {1}", _property.PropertyType.FullName, _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tsubject.{0} = value;", _property.Name,
                             _property.PropertyType.FullName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        public PropertyInfo Property
        {
            set { _property = value; }
        }
    }
}
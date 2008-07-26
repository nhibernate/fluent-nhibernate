using System;
using System.IO;
using System.Reflection;

namespace ShadeTree.DomainModel.Generation
{
    public class GenericSimpleProperty : IProperty
    {
        private PropertyInfo _property;
        private Type _propertyType;

        public GenericSimpleProperty()
        {
        }

        #region IProperty Members

        public void WritePropertiesForDomainClassFixture(TextWriter writer)
        {
            if (_property.CanWrite)
            {
                writer.WriteLine();
                writer.WriteLine("\t\t[Example(\"|{0} is|[value]|\")]", _property.Name);
                writer.WriteLine("\t\tpublic void {0}Is({1}? propertyValue)", _property.Name, _propertyType.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tSubject.{0} = propertyValue;", _property.Name);
                writer.WriteLine("\t\t}");

                writer.WriteLine();
                writer.WriteLine("\t\t[Example(\"|{0} is null|\")]", _property.Name);
                writer.WriteLine("\t\tpublic void {0}IsNull()", _property.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tSubject.{0} = null;", _property.Name);
                writer.WriteLine("\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t[Example(\"|Check|{0} is|[value]|\")]", _property.Name);
            writer.WriteLine("\t\tpublic {0} {1}Is()", _propertyType, _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn Subject.{0}.Value;", _property.Name);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine();
            writer.WriteLine("\t\t[Example(\"|Check|that {0} is null|\")]", _property.Name);
            writer.WriteLine("\t\tpublic bool That{0}IsNull()", _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn !Subject.{0}.HasValue;", _property.Name);
            writer.WriteLine("\t\t}");
        }


        public void WritePropertiesForListFixture(TextWriter writer)
        {
            if (!_property.CanWrite)
            {
                return;
            }

            writer.WriteLine();
            writer.WriteLine("\t\tpublic {0}? {1}", _propertyType.FullName, _property.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tsubject.{0} = value;", _property.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        public PropertyInfo Property
        {
            set
            {
                _property = value;
                _propertyType = value.PropertyType.GetGenericArguments()[0];
                ;
            }
        }

        #endregion
    }
}
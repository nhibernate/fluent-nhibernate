using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ICollection=System.Collections.ICollection;

namespace ShadeTree.DomainModel.Generation
{
    public interface IWriteableWithTypes : IWriteable
    {
        void RegisterTypes(Action<Type> action);
    }

    public interface IWriteable
    {
        void Write(TextWriter writer);
    }

    public class DomainFixture : IWriteable
    {
        private readonly Type _type;
        private readonly List<IProperty> _properties = new List<IProperty>();

        public DomainFixture(Type type, PropertyBuilder builder)
        {
            _type = type;

            foreach (PropertyInfo property in _type.GetProperties())
            {
                IProperty prop = builder.Build(property);
                _properties.Add(prop);
            }
        }


        public void Write(TextWriter writer)
        {
            writeTheInstanceFixture(writer);
            writeTheListFixture(writer);
        }

        private void writeTheListFixture(TextWriter writer)
        {


            writer.WriteLine();
            writer.WriteLine("\tpublic partial class {0}ListFixture : DomainListFixture<{0}>", _type.Name);
            writer.WriteLine("\t{");

            writer.WriteLine();
            writer.WriteLine("\t\tpublic {0}ListFixture() : base(){{}}", _type.Name);
            writer.WriteLine("\t\tpublic {0}ListFixture(Action<{0}[]> doneAction) : base(doneAction){{}}", _type.Name);
            writer.WriteLine();

            foreach (IProperty property in _properties)
            {
                property.WritePropertiesForListFixture(writer);
            }


            writer.WriteLine("\t}");
        }

        private void writeTheInstanceFixture(TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("\tpublic partial class {0}Fixture : DomainClassFixture<{0}>", _type.Name);
            writer.WriteLine("\t{");

            writer.WriteLine();
            writer.WriteLine("\t\tpublic {0}Fixture() : base(){{}}", _type.Name);
            writer.WriteLine("\t\tpublic {0}Fixture({0} subject) : base(subject){{}}", _type.Name);
            writer.WriteLine();

            foreach (IProperty property in _properties)
            {
                property.WritePropertiesForDomainClassFixture(writer);
            }


            writer.WriteLine("\t}");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using fit;
using fitlibrary;
using FluentNHibernate.Fixtures;
using FluentNHibernate.Fixtures;

namespace FluentNHibernate.Generation
{
    public class FixtureFileDefinition
    {
        private PropertyBuilder _propertyBuilder;
        public string FilePath { get; set; }
        public string Namespace { get; set; }
        public Assembly Assembly {get; set;}
        public Func<Type, bool> TypeFilter { get; set; }

        public PropertyBuilder PropertyBuilder
        {
            get { return _propertyBuilder; }
        }

        public FixtureFileDefinition()
        {
            _propertyBuilder = new PropertyBuilder();
        }

        public void GenerateFile()
        {
            CodeFile file = new CodeFile(FilePath, Namespace);

            file.AddNamespace(typeof(DoFixture));
            file.AddNamespace(typeof(Fixture));
            file.AddNamespace(typeof(FluentNHibernate.FixtureModel.ExampleAttribute));
            file.AddNamespace(typeof(DomainFixture));
            file.AddNamespace(typeof(DomainClassFixture<>));

            file.PropertyBuilder = _propertyBuilder;

            foreach (Type exportedType in Assembly.GetExportedTypes())
            {
                if (exportedType.IsAbstract || exportedType.IsInterface) continue;

                if (TypeFilter(exportedType))
                {
                    file.AddDomainType(exportedType);
                }
            }

            file.WriteToFile();
        }
    }

    public class CodeFile
    {
        private readonly List<string> _namespaces = new List<string>();
        private string _fileName;
        private readonly string _nameSpace;
        private List<IWriteable> _fixtures = new List<IWriteable>();
        private PropertyBuilder _propertyBuilder = new PropertyBuilder();

        public CodeFile(string fileName, string nameSpace)
        {
            _fileName = fileName;
            _nameSpace = nameSpace;

        }

        public PropertyBuilder PropertyBuilder
        {
            get { return _propertyBuilder; }
            set { _propertyBuilder = value; }
        }

        public void AddNamespace(string namespaceName)
        {
            if (namespaceName == this.GetType().Namespace)
            {
                return;
            }

            if (!_namespaces.Contains(namespaceName))
            {
                _namespaces.Add(namespaceName);
            }
        }

        public void AddNamespace(Type type)
        {
            AddNamespace(type.Namespace);
            foreach (PropertyInfo property in type.GetProperties())
            {
                AddNamespace(property.PropertyType.Namespace);
            }
        }

        public void AddDomainType<T>()
        {
            Type type = typeof(T);
            AddDomainType(type);
        }

        public void AddDomainType(Type type)
        {
            AddNamespace(type);
            DomainFixture fixture = new DomainFixture(type, _propertyBuilder);
            _fixtures.Add(fixture);
        }

        public void AddWriteable(IWriteable writeable)
        {
            var withTypes = writeable as IWriteableWithTypes;
            if (withTypes != null) withTypes.RegisterTypes(AddNamespace);

            _fixtures.Add(writeable);
        }

        public void WriteToConsole()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            writeAll(writer);

            Console.WriteLine(sb.ToString());
        }

        public string Write()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            writeAll(writer);

            return sb.ToString();
        }

        private void writeAll(TextWriter writer)
        {
            _namespaces.Sort();
            foreach (string namespaceName in _namespaces)
            {
                writer.WriteLine("using {0};", namespaceName);
            }

            writer.WriteLine();
            writer.WriteLine("namespace {0}", _nameSpace);
            writer.WriteLine("{");

            foreach (var fixture in _fixtures)
            {
                fixture.Write(writer);
                writer.WriteLine();
            }

            writer.WriteLine("}");
        }

        public void WriteToFile()
        {
            Console.WriteLine("Writing file " + _fileName);
            using (StreamWriter writer = new StreamWriter(_fileName, false))
            {
                writeAll(writer);
            }
        }
    }
}
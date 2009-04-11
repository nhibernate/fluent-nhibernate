using System;
using System.Linq;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    public class ModelTester<T>
    {
        protected XmlElement currentElement;
        protected XmlDocument document;
        protected IMappingVisitor _visitor;
        private readonly PersistenceModel model;
        private HibernateMapping hbm;

        public ModelTester()
        {
            model = new PersistenceModel();
            _visitor = new MappingVisitor(new Configuration());
        }

        public virtual ModelTester<T> Conventions(Action<IConventionFinder> conventionFinderAction)
        {
            conventionFinderAction(model.ConventionFinder);
            return this;
        }

        public virtual ModelTester<T> ForMapping(Action<ClassMap<T>> mappingAction)
        {
            var classMap = new ClassMap<T>();
            mappingAction(classMap);

            return ForMapping(classMap);
        }

        public virtual ModelTester<T> ForMapping(ClassMap<T> classMap)
        {
            model.Add(classMap);
            model.ApplyConventions();

            hbm = model.BuildHibernateMapping();

            return this;
        }

        public virtual ModelTester<T> Verify(Action<HibernateMapping> hbmAction)
        {
            hbmAction(hbm);

            return this;
        }

        public virtual ModelTester<T> VerifyClass(Action<ClassMapping> classAction)
        {
            var clazz = hbm.Classes.FirstOrDefault(x => x.Type == typeof(T));

            clazz.ShouldNotBeNull();

            classAction(clazz);

            return this;
        }
    }
}
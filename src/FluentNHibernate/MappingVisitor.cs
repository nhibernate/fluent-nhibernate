using System;
using System.Xml;
using FluentNHibernate.Metadata;
using NHibernate.Cfg;
using FluentNHibernate.Metadata;

namespace FluentNHibernate
{
    public class MappingVisitor : IMappingVisitor
    {
        private readonly Configuration _configuration;
        private readonly DependencyChain _chain;
        private readonly Conventions _conventions;
        

        public MappingVisitor(Conventions conventions, Configuration configuration, DependencyChain chain)
        {
            _conventions = conventions;
            _configuration = configuration;
            _chain = chain;
        }

		public MappingVisitor() : this(new Conventions(), new Configuration(), new DependencyChain())
		{
		}

        #region IMappingVisitor Members

        public Conventions Conventions
        {
            get { return _conventions; }
        }

        public Type CurrentType { get; set;}


        public virtual void AddMappingDocument(XmlDocument document, Type type)
        {
            _configuration.AddDocument(document);
        }

        public void RegisterDependency(Type parentType)
        {
            _chain.RegisterDependency(CurrentType, parentType);
        }

        #endregion
    }
}

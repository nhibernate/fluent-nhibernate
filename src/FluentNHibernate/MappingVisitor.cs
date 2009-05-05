using System;
using System.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class MappingVisitor : IMappingVisitor
    {
        private readonly Configuration configuration;

        public MappingVisitor(Configuration configuration)
        {
            this.configuration = configuration;
        }

		public MappingVisitor() : this(new Configuration())
		{
		}

        #region IMappingVisitor Members

        public Type CurrentType { get; set;}

        public virtual void AddMappingDocument(XmlDocument document, Type type)
        {
            if (configuration.GetClassMapping(type) == null)
                configuration.AddDocument(document);
        }

        #endregion
    }
}

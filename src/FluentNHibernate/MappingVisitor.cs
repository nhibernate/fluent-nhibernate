using System;
using System.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class MappingVisitor : IMappingVisitor
    {
        private readonly Configuration _configuration;

        public MappingVisitor(Configuration configuration)
        {
            _configuration = configuration;
        }

		public MappingVisitor() : this(new Configuration())
		{
		}

        #region IMappingVisitor Members

        public Type CurrentType { get; set;}

        public virtual void AddMappingDocument(XmlDocument document, Type type)
        {
            if (_configuration.GetClassMapping(type) == null)
                _configuration.AddDocument(document);
        }

        #endregion
    }
}

using System.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate.MappingModel.Output
{
    /// <summary>
    /// A mapping application strategy which utilizes <see cref="MappingXmlSerializer"/> for translation.
    /// </summary>
    public class XmlMappingApplicationStrategy : MappingApplicationStrategyBase<XmlDocument>
    {
        public XmlMappingApplicationStrategy()
        {
        }

        protected override XmlDocument ToIntermediateForm(HibernateMapping mapping)
        {
            return new MappingXmlSerializer().Serialize(mapping);
        }

        protected override void ApplyIntermediateFormToConfiguration(XmlDocument intermediateForm, Configuration cfg)
        {
            cfg.AddDocument(intermediateForm);
        }
    }
}

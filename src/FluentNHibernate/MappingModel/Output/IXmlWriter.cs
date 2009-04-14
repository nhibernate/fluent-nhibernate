using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public interface IXmlWriter<T> //: IMappingModelVisitor
    {
        object Write(T mappingModel);        
    }
}
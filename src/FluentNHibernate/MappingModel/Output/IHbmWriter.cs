using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{

    public interface IHbmWriter<T> : IMappingModelVisitor
    {
        object Write(T mappingModel);        
    }    

}
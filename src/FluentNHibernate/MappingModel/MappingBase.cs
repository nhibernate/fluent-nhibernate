using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public abstract class MappingBase : IMappingBase
    {
        public abstract void AcceptVisitor(IMappingModelVisitor visitor);        
    }
}
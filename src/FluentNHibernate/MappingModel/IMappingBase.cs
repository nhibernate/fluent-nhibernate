using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public interface IMappingBase
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
    }
}
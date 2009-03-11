using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    public interface IAssemblyConvention : IConvention<IEnumerable<IClassMap>>
    {}
}
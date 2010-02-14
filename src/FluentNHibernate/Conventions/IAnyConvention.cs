using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IAnyConvention : IConvention<IAnyInspector, IAnyInstance>
    {
        
    }
}
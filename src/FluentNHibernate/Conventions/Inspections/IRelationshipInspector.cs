using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IRelationshipInspector : IInspector
    {
        TypeReference Class { get; }

		void CustomClass<T>();

		void CustomClass(Type type);
	}
}
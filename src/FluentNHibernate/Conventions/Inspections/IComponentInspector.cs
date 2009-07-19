using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IComponentBaseInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string ParentName { get; }
        bool Insert();
        bool Update();
    }

    public interface IComponentInspector : IComponentBaseInspector
    {}

    public interface IDynamicComponentInspector: IComponentBaseInspector
    {}

    public class ComponentInspector : IComponentBaseInspector
    {
        private readonly IComponentMapping mapping;

        public ComponentInspector(IComponentMapping mapping)
        {
            this.mapping = mapping;
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public PropertyInfo Property
        {
            get { throw new NotImplementedException(); }
        }
        public string ParentName
        {
            get { throw new NotImplementedException(); }
        }
        public bool Insert()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    internal class MappedMembers : IMappingBase, IHasMappedMembers
    {
        private readonly List<PropertyMapping> properties;
        private readonly List<ICollectionMapping> collections;
        private readonly List<ManyToOneMapping> references;
        private readonly List<IComponentMapping> components;
        private readonly List<OneToOneMapping> oneToOnes;
        private readonly List<AnyMapping> anys;
        private readonly List<JoinMapping> joins;

        public MappedMembers()
        {
            properties = new List<PropertyMapping>();
            collections = new List<ICollectionMapping>();
            references = new List<ManyToOneMapping>();
            components = new List<IComponentMapping>();
            oneToOnes = new List<OneToOneMapping>();
            anys = new List<AnyMapping>();
            joins = new List<JoinMapping>();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return properties; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return collections; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return references; }
        }

        public IEnumerable<IComponentMapping> Components
        {
            get { return components; }
        }

        public IEnumerable<OneToOneMapping> OneToOnes
        {
            get { return oneToOnes; }
        }

        public IEnumerable<AnyMapping> Anys
        {
            get { return anys; }
        }

        public IEnumerable<JoinMapping> Joins
        {
            get { return joins; }
        }

        public void AddProperty(PropertyMapping property)
        {
            if (properties.Exists(x => x.Name == property.Name))
                throw new InvalidOperationException("Tried to add property '" + property.Name + "' when already added.");

            properties.Add(property);
        }

        public void AddOrReplaceProperty(PropertyMapping mapping)
        {
            properties.RemoveAll(x => x.Name == mapping.Name);
            properties.Add(mapping);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            if (collections.Exists(x => x.Name == collection.Name))
                throw new InvalidOperationException("Tried to add collection '" + collection.Name + "' when already added.");

            collections.Add(collection);
        }

        public void AddOrReplaceCollection(ICollectionMapping mapping)
        {
            collections.RemoveAll(x => x.Name == mapping.Name);
            collections.Add(mapping);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            if (references.Exists(x => x.Name == manyToOne.Name))
                throw new InvalidOperationException("Tried to add many-to-one '" + manyToOne.Name + "' when already added.");

            references.Add(manyToOne);
        }

        public void AddOrReplaceReference(ManyToOneMapping manyToOne)
        {
            references.RemoveAll(x => x.Name == manyToOne.Name);
            references.Add(manyToOne);
        }

        public void AddComponent(IComponentMapping componentMapping)
        {
            if (components.Exists(x => x.Name == componentMapping.Name))
                throw new InvalidOperationException("Tried to add component '" + componentMapping.Name + "' when already added.");

            components.Add(componentMapping);
        }

        public void AddOrReplaceComponent(IComponentMapping componentMapping)
        {
            components.RemoveAll(x => x.Name == componentMapping.Name);
            components.Add(componentMapping);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            if (oneToOnes.Exists(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add one-to-one '" + mapping.Name + "' when already added.");

            oneToOnes.Add(mapping);
        }

        public void AddOrReplaceOneToOne(OneToOneMapping mapping)
        {
            oneToOnes.RemoveAll(x => x.Name == mapping.Name);
            oneToOnes.Add(mapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            if (anys.Exists(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add any '" + mapping.Name + "' when already added.");

            anys.Add(mapping);
        }

        public void AddOrReplaceAny(AnyMapping mapping)
        {
            anys.RemoveAll(x => x.Name == mapping.Name);
            anys.Add(mapping);
        }

        public void AddJoin(JoinMapping mapping)
        {
            if (joins.Exists(x => x.TableName == mapping.TableName))
                throw new InvalidOperationException("Tried to add join to table '" + mapping.TableName + "' when already added.");

            joins.Add(mapping);
        }

        public virtual void AcceptVisitor(IMappingModelVisitor visitor)
        {
            foreach (var collection in Collections)
                visitor.Visit(collection);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);

            foreach (var component in Components)
                visitor.Visit(component);

            foreach (var oneToOne in oneToOnes)
                visitor.Visit(oneToOne);

            foreach (var any in anys)
                visitor.Visit(any);

            foreach (var join in joins)
                visitor.Visit(join);
        }
    }
}

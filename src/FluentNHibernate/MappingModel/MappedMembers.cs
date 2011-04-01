using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    internal class MappedMembers : IMappingBase, IHasMappedMembers
    {
        private readonly List<PropertyMapping> properties;
        private readonly List<Collections.CollectionMapping> collections;
        private readonly List<ManyToOneMapping> references;
        private readonly List<IComponentMapping> components;
        private readonly List<OneToOneMapping> oneToOnes;
        private readonly List<AnyMapping> anys;
        private readonly List<JoinMapping> joins;
        private readonly List<FilterMapping> filters;
        private readonly List<StoredProcedureMapping> storedProcedures;

        public MappedMembers()
        {
            properties = new List<PropertyMapping>();
            collections = new List<Collections.CollectionMapping>();
            references = new List<ManyToOneMapping>();
            components = new List<IComponentMapping>();
            oneToOnes = new List<OneToOneMapping>();
            anys = new List<AnyMapping>();
            joins = new List<JoinMapping>();
            filters = new List<FilterMapping>();
            storedProcedures = new List<StoredProcedureMapping>();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return properties; }
        }

        public IEnumerable<Collections.CollectionMapping> Collections
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

        public IEnumerable<FilterMapping> Filters
        {
            get { return filters; }
        }

        public void AddOrReplaceFilter(FilterMapping mapping)
        {
            var filter = filters.Find(x => x.Name == mapping.Name);
            if (filter != null)
                filters.Remove(filter);
            filters.Add(mapping);
        }


        public IEnumerable<StoredProcedureMapping> StoredProcedures
        {
            get { return storedProcedures; }
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

        public void AddCollection(Collections.CollectionMapping collection)
        {
            if (collections.Exists(x => x.Name == collection.Name))
                throw new InvalidOperationException("Tried to add collection '" + collection.Name + "' when already added.");

            collections.Add(collection);
        }

        public void AddOrReplaceCollection(Collections.CollectionMapping mapping)
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

        public void AddFilter(FilterMapping mapping)
        {
            if (filters.Exists(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add filter with name '" + mapping.Name + "' when already added.");

            filters.Add(mapping);
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

            foreach (var filter in filters)
                visitor.Visit(filter);

            foreach (var storedProcedure in storedProcedures)
                visitor.Visit(storedProcedure);
        }

        public bool IsSpecified(string property)
        {
            return false;
        }

        public void AddStoredProcedure(StoredProcedureMapping mapping)
        {
            storedProcedures.Add(mapping);
        }

        public bool Equals(MappedMembers other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.properties.ContentEquals(properties) &&
                other.collections.ContentEquals(collections) &&
                other.references.ContentEquals(references) &&
                other.components.ContentEquals(components) &&
                other.oneToOnes.ContentEquals(oneToOnes) &&
                other.anys.ContentEquals(anys) &&
                other.joins.ContentEquals(joins) &&
                other.filters.ContentEquals(filters) &&
                other.storedProcedures.ContentEquals(storedProcedures);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MappedMembers)) return false;
            return Equals((MappedMembers)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (properties != null ? properties.GetHashCode() : 0);
                result = (result * 397) ^ (collections != null ? collections.GetHashCode() : 0);
                result = (result * 397) ^ (references != null ? references.GetHashCode() : 0);
                result = (result * 397) ^ (components != null ? components.GetHashCode() : 0);
                result = (result * 397) ^ (oneToOnes != null ? oneToOnes.GetHashCode() : 0);
                result = (result * 397) ^ (anys != null ? anys.GetHashCode() : 0);
                result = (result * 397) ^ (joins != null ? joins.GetHashCode() : 0);
                result = (result * 397) ^ (filters != null ? filters.GetHashCode() : 0);
                result = (result * 397) ^ (storedProcedures != null ? storedProcedures.GetHashCode() : 0);
                return result;
            }
        }

    }
}

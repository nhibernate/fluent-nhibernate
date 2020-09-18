using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    internal class MappedMembers : IMapping, IHasMappedMembers
    {
        public enum MappingType {
            Property,
            Collection,
            ManyToOne,
            IComponent,
            OneToOne,
            Any,
            Join,
            Filter,
            StoredProcedure,
        }

        private readonly List<Tuple<MappingType, IMapping>> orderedMappings;

        public MappedMembers()
        {
            orderedMappings = new List<Tuple<MappingType, IMapping>>();
        }

        public IEnumerable<PropertyMapping> Properties => orderedMappings.Where(x => x.Item1 == MappingType.Property).Select(x => x.Item2).Cast<PropertyMapping>();

        public IEnumerable<Collections.CollectionMapping> Collections => orderedMappings.Where(x => x.Item1 == MappingType.Collection).Select(x => x.Item2).Cast<Collections.CollectionMapping>();

        public IEnumerable<ManyToOneMapping> References => orderedMappings.Where(x => x.Item1 == MappingType.ManyToOne).Select(x => x.Item2).Cast<ManyToOneMapping>();

        public IEnumerable<IComponentMapping> Components => orderedMappings.Where(x => x.Item1 == MappingType.IComponent).Select(x => x.Item2).Cast<IComponentMapping>();

        public IEnumerable<OneToOneMapping> OneToOnes => orderedMappings.Where(x => x.Item1 == MappingType.OneToOne).Select(x => x.Item2).Cast<OneToOneMapping>();

        public IEnumerable<AnyMapping> Anys => orderedMappings.Where(x => x.Item1 == MappingType.Any).Select(x => x.Item2).Cast<AnyMapping>();

        public IEnumerable<JoinMapping> Joins => orderedMappings.Where(x => x.Item1 == MappingType.Join).Select(x => x.Item2).Cast<JoinMapping>();

        public IEnumerable<FilterMapping> Filters => orderedMappings.Where(x => x.Item1 == MappingType.Filter).Select(x => x.Item2).Cast<FilterMapping>();

        public IEnumerable<StoredProcedureMapping> StoredProcedures => orderedMappings.Where(x => x.Item1 == MappingType.StoredProcedure).Select(x => x.Item2).Cast<StoredProcedureMapping>();

        public void AddOrReplaceFilter(FilterMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Filter, x => x.Name == mapping.Name);
        }

        public void AddProperty(PropertyMapping property)
        {
            if (Properties.Any(x => x.Name == property.Name))
                throw new InvalidOperationException("Tried to add property '" + property.Name + "' when already added.");
            AddMapping(property, MappingType.Property);
        }

        public void AddOrReplaceProperty(PropertyMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Property, x => x.Name == mapping.Name);
        }

        public void AddCollection(Collections.CollectionMapping collection)
        {
            if (Collections.Any(x => x.Name == collection.Name))
                throw new InvalidOperationException("Tried to add collection '" + collection.Name + "' when already added.");
            AddMapping(collection, MappingType.Collection);
        }

        public void AddOrReplaceCollection(Collections.CollectionMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Collection, x => x.Name == mapping.Name);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            if (References.Any(x => x.Name == manyToOne.Name))
                throw new InvalidOperationException("Tried to add many-to-one '" + manyToOne.Name + "' when already added.");
            AddMapping(manyToOne, MappingType.ManyToOne);
        }

        public void AddOrReplaceReference(ManyToOneMapping manyToOne)
        {
            AddOrReplaceMapping(manyToOne, MappingType.ManyToOne, x => x.Name == manyToOne.Name);
        }

        public void AddComponent(IComponentMapping componentMapping)
        {
            if (Components.Any(x => x.Name == componentMapping.Name))
                throw new InvalidOperationException("Tried to add component '" + componentMapping.Name + "' when already added.");
            AddMapping(componentMapping, MappingType.IComponent);
        }

        public void AddOrReplaceComponent(IComponentMapping componentMapping)
        {
            AddOrReplaceMapping(componentMapping, MappingType.IComponent, x => x.Name == componentMapping.Name);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            if (OneToOnes.Any(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add one-to-one '" + mapping.Name + "' when already added.");
            AddMapping(mapping, MappingType.OneToOne);
        }

        public void AddOrReplaceOneToOne(OneToOneMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.OneToOne, x => x.Name == mapping.Name);
        }

        public void AddAny(AnyMapping mapping)
        {
            if (Anys.Any(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add any '" + mapping.Name + "' when already added.");
            AddMapping(mapping, MappingType.Any);
        }

        public void AddOrReplaceAny(AnyMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Any, x => x.Name == mapping.Name);
        }

        public void AddJoin(JoinMapping mapping)
        {
            if (Joins.Any(x => x.TableName == mapping.TableName))
                throw new InvalidOperationException("Tried to add join to table '" + mapping.TableName + "' when already added.");
            AddMapping(mapping, MappingType.Join);
        }

        public void AddOrReplaceJoin(JoinMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Join, x => x.TableName == mapping.TableName);
        }

        public void AddFilter(FilterMapping mapping)
        {
            if (Filters.Any(x => x.Name == mapping.Name))
                throw new InvalidOperationException("Tried to add filter with name '" + mapping.Name + "' when already added.");
            AddMapping(mapping, MappingType.Filter);
        }

        public virtual void AcceptVisitor(IMappingModelVisitor visitor)
        {
            foreach(var mapping in orderedMappings)
                switch (mapping.Item1) {
                    case MappingType.Property:
                        visitor.Visit((PropertyMapping)mapping.Item2);
                        break;
                    case MappingType.Collection:
                        visitor.Visit((Collections.CollectionMapping)mapping.Item2);
                        break;
                    case MappingType.ManyToOne:
                        visitor.Visit((ManyToOneMapping)mapping.Item2);
                        break;
                    case MappingType.IComponent:
                        visitor.Visit((IComponentMapping)mapping.Item2);
                        break;
                    case MappingType.OneToOne:
                        visitor.Visit((OneToOneMapping)mapping.Item2);
                        break;
                    case MappingType.Any:
                        visitor.Visit((AnyMapping)mapping.Item2);
                        break;
                    case MappingType.Join:
                        visitor.Visit((JoinMapping)mapping.Item2);
                        break;
                    case MappingType.Filter:
                        visitor.Visit((FilterMapping)mapping.Item2);
                        break;
                    case MappingType.StoredProcedure:
                        visitor.Visit((StoredProcedureMapping)mapping.Item2);
                        break;
                    default:
                        throw new Exception("Internal Error: unsupported mapping type " + mapping.Item1);
                }
        }

        public bool IsSpecified(string property)
        {
            return false;
        }

        public void Set(string attribute, int layer, object value)
        {}

        public void AddStoredProcedure(StoredProcedureMapping mapping)
        {
            AddMapping(mapping, MappingType.StoredProcedure);
        }

        public bool Equals(MappedMembers other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.orderedMappings.ContentEquals(orderedMappings);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MappedMembers)) return false;
            return Equals((MappedMembers)obj);
        }

        public override int GetHashCode() {
            return orderedMappings.GetHashCode();
        }

        private void AddMapping<TMapping>(TMapping mapping, MappingType mappingType) where TMapping : IMapping {
            orderedMappings.Add(Tuple.Create(mappingType, (IMapping)mapping));
        }

        private void AddOrReplaceMapping<TMapping>(TMapping mapping, MappingType mappingType, Predicate<TMapping> matchPredicate) {
            var newMapping = Tuple.Create(mappingType, (IMapping)mapping);            
            var index = orderedMappings.FindIndex(x => x.Item1 == mappingType && matchPredicate((TMapping)x.Item2));
            if (index >= 0)
                orderedMappings[index] = newMapping;
            else
                orderedMappings.Add(newMapping);
        }

    }
}


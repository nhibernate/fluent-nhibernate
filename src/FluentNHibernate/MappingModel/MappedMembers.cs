using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using NHibernate.Util;

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
        private readonly IReadOnlyDictionary<MappingType, IDictionary<string, int>> mappingIndicesByType;

        public MappedMembers()
        {
            orderedMappings = new List<Tuple<MappingType, IMapping>>();
            // This has to use a NullableDictionary or (at least) various test cases will fail due to null names on mappings
            IDictionary<string, int> IndexMapGen(MappingType ignored) => new NullableDictionary<string, int>();
            mappingIndicesByType = Enum.GetValues(typeof(MappingType)).Cast<MappingType>().ToDictionary(m => m, IndexMapGen);
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
            AddOrReplaceMapping(mapping, MappingType.Filter, x => x.Name);
        }

        public void AddProperty(PropertyMapping property)
        {
            AddMapping(property, MappingType.Property, x => x.Name,
                "Tried to add property '" + property.Name + "' when already added.");
        }

        public void AddOrReplaceProperty(PropertyMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Property, x => x.Name);
        }

        public void AddCollection(Collections.CollectionMapping collection)
        {
            AddMapping(collection, MappingType.Collection, x => x.Name,
                "Tried to add collection '" + collection.Name + "' when already added.");
        }

        public void AddOrReplaceCollection(Collections.CollectionMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Collection, x => x.Name);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            AddMapping(manyToOne, MappingType.ManyToOne, x => x.Name,
                "Tried to add many-to-one '" + manyToOne.Name + "' when already added.");
        }

        public void AddOrReplaceReference(ManyToOneMapping manyToOne)
        {
            AddOrReplaceMapping(manyToOne, MappingType.ManyToOne, x => x.Name);
        }

        public void AddComponent(IComponentMapping componentMapping)
        {
            AddMapping(componentMapping, MappingType.IComponent, x => x.Name,
                "Tried to add component '" + componentMapping.Name + "' when already added.");
        }

        public void AddOrReplaceComponent(IComponentMapping componentMapping)
        {
            AddOrReplaceMapping(componentMapping, MappingType.IComponent, x => x.Name);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            AddMapping(mapping, MappingType.OneToOne, x => x.Name,
                "Tried to add one-to-one '" + mapping.Name + "' when already added.");
        }

        public void AddOrReplaceOneToOne(OneToOneMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.OneToOne, x => x.Name);
        }

        public void AddAny(AnyMapping mapping)
        {
            AddMapping(mapping, MappingType.Any, x => x.Name,
                "Tried to add any '" + mapping.Name + "' when already added.");
        }

        public void AddOrReplaceAny(AnyMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Any, x => x.Name);
        }

        public void AddJoin(JoinMapping mapping)
        {
            AddMapping(mapping, MappingType.Join, x => x.TableName,
                "Tried to add join to table '" + mapping.TableName + "' when already added.");
        }

        public void AddOrReplaceJoin(JoinMapping mapping)
        {
            AddOrReplaceMapping(mapping, MappingType.Join, x => x.TableName);
        }

        public void AddFilter(FilterMapping mapping)
        {
            AddMapping(mapping, MappingType.Filter, x => x.Name,
                "Tried to add filter with name '" + mapping.Name + "' when already added.");
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
            ManageMapping(mapping, MappingType.StoredProcedure, null, true, false, null);
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

        private void AddMapping<TMapping>(TMapping mapping, MappingType mappingType, Func<TMapping, string> keyMapper, string badDupeMsg)
                where TMapping : IMapping
        {
            ManageMapping(mapping, mappingType, keyMapper, false, false, badDupeMsg);
        }

        private void AddOrReplaceMapping<TMapping>(TMapping mapping, MappingType mappingType, Func<TMapping, string> keyMapper) where TMapping : IMapping
        {
            ManageMapping(mapping, mappingType, keyMapper, false, true, null);
        }

        private void ManageMapping<TMapping>(TMapping mapping, MappingType mappingType, Func<TMapping, string> keyMapper, bool allowDupes, bool allowReplace,
                string badDupeMsg) where TMapping : IMapping
        {
            var newTuple = Tuple.Create(mappingType, (IMapping)mapping);
            if (allowDupes) {
                orderedMappings.Add(newTuple);
                // Types that allow duplicates don't have index tracking
            } else {
                var key = keyMapper(mapping);
                var mappingIndices = mappingIndicesByType[mappingType];
                if (!mappingIndices.ContainsKey(key))
                {
                    orderedMappings.Add(newTuple);
                    mappingIndices[key] = orderedMappings.Count - 1;
                }
                else if (allowReplace)
                        orderedMappings[mappingIndices[key]] = newTuple;
                else
                    throw new InvalidOperationException(badDupeMsg);
            }
        }
    }
}

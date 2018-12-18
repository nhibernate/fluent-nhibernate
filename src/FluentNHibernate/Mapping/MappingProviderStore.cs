using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class MappingProviderStore
    {
        public enum ProviderType {
            Property,
            Component,
            OneToOne,
            Subclass,
            Collection,
            ManyToOne,
            Any,
            Filter,
            StoredProcedure,
            Join,
            Identity,
            CompositeId,
            NaturalId,
            Version,
            Discriminator,
            Tupilizer
        }

        public IList<IPropertyMappingProvider> Properties { get; set; }
        public IList<IComponentMappingProvider> Components { get; set; }
        public IList<IOneToOneMappingProvider> OneToOnes { get; set; }
        public IDictionary<Type, ISubclassMappingProvider> Subclasses { get; set; }
        public IList<ICollectionMappingProvider> Collections { get; set; }
        public IList<IManyToOneMappingProvider> References { get; set; }
        public IList<IAnyMappingProvider> Anys { get; set; }
        public IList<IFilterMappingProvider> Filters { get; set; }
        public IList<IStoredProcedureMappingProvider> StoredProcedures { get; set; }
        public IList<IJoinMappingProvider> Joins { get; set; }
        private IIdentityMappingProvider _id;
        private ICompositeIdMappingProvider _compositeId;
        private INaturalIdMappingProvider _naturalId;
        private IVersionMappingProvider _version;
        private IDiscriminatorMappingProvider _discriminator;
        private TuplizerMapping _tuplizerMapping;
        private IList<Tuple<ProviderType, object>> orderedProviders;


        public IIdentityMappingProvider Id {
            get {
                return _id;
            }
            set {
                ReplaceOrAddProvider(ProviderType.Identity, _id, value);
                _id = value;
            }
        }

        public ICompositeIdMappingProvider CompositeId {
            get {
                return _compositeId;
            }
            set {
                ReplaceOrAddProvider(ProviderType.CompositeId, _compositeId, value);
                _compositeId = value;
            }
        }

        public INaturalIdMappingProvider NaturalId {
            get {
                return _naturalId;
            }
            set {
                ReplaceOrAddProvider(ProviderType.NaturalId, _naturalId, value);
                _naturalId = value;
            }
        }

        public IVersionMappingProvider Version {
            get {
                return _version;
            }
            set {
                ReplaceOrAddProvider(ProviderType.Version, _version, value);
                _version = value;
            }
        }

        public IDiscriminatorMappingProvider Discriminator {
            get {
                return _discriminator;
            }
            set {
                ReplaceOrAddProvider(ProviderType.Discriminator, _discriminator, value);
                _discriminator = value;
            }
        }

        public TuplizerMapping TuplizerMapping {
            get {
                return _tuplizerMapping;
            }
            set {
                ReplaceOrAddProvider(ProviderType.Tupilizer, _tuplizerMapping, value);
                _tuplizerMapping = value;
            }
        }

        public MappingProviderStore()
        {            
            Properties = NewObservedList<IPropertyMappingProvider>();
            Components = NewObservedList<IComponentMappingProvider>();
            OneToOnes = NewObservedList<IOneToOneMappingProvider>();
            Subclasses = NewObservedDictionary<Type, ISubclassMappingProvider>();
            Collections = NewObservedList<ICollectionMappingProvider>();
            References = NewObservedList<IManyToOneMappingProvider>();
            Anys = NewObservedList<IAnyMappingProvider>();
            Filters = NewObservedList<IFilterMappingProvider>();
            StoredProcedures = NewObservedList<IStoredProcedureMappingProvider>();
            Joins = NewObservedList<IJoinMappingProvider>();
            orderedProviders = new List<Tuple<ProviderType, object>>();
        }

        public IEnumerable<Tuple<ProviderType, object>> OrderedProviders {
            get { return orderedProviders.Select(x => x); }
        }

        private IList<T> NewObservedList<T>() {

            ProviderType TypeSelector(object o) 
            {
                if (ReferenceEquals(o, Properties)) {
                    return ProviderType.Property;
                } else if (ReferenceEquals(o, Components)) {
                    return ProviderType.Component;
                } else if (ReferenceEquals(o, OneToOnes)) {
                    return ProviderType.OneToOne;
                } else if (ReferenceEquals(o, Collections)) {
                    return ProviderType.Collection;
                } else if (ReferenceEquals(o, References)) {
                    return ProviderType.ManyToOne;
                } else if (ReferenceEquals(o, Anys)) {
                    return ProviderType.Any;
                } else if (ReferenceEquals(o, Filters)) {
                    return ProviderType.Filter;
                } else if (ReferenceEquals(o, StoredProcedures)) {
                    return ProviderType.StoredProcedure;
                } else if (ReferenceEquals(o, Joins)) {
                    return ProviderType.Join;
                }
                throw new Exception("Internal Error");
            }

            var observableList = new ObservableCollection<T>();
            observableList.CollectionChanged += (sender, args) => {
                var type = TypeSelector(sender);
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in args.NewItems)
                            orderedProviders.Add(Tuple.Create(type, newItem));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException();

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var oldItem in args.OldItems)
                            orderedProviders.Remove(Tuple.Create(type, oldItem));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        throw new NotSupportedException();
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var oldItem in args.OldItems)
                            orderedProviders.Remove(Tuple.Create(type, oldItem));
                        break;
                }
            };
            return observableList;
        }

        private IDictionary<TKey, TVal> NewObservedDictionary<TKey, TVal>() {
            var observedDictionary = new ObservableDictionary<TKey,TVal>();
            observedDictionary.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        if (args.NewStartingIndex < ((IDictionary<TKey, TVal>)sender).Count) {
                            //Inserting
                            for (var i = 0; i < args.NewItems.Count; i++) {
                                var newValue = (KeyValuePair<TKey, TVal>)args.NewItems[i];
                                orderedProviders.Insert(args.NewStartingIndex + i, Tuple.Create(ProviderType.Subclass, (object)newValue.Value));
                            }
                        } else {
                            //Appending
                            foreach (KeyValuePair<TKey, TVal> newItem in args.NewItems) {
                                orderedProviders.Add(Tuple.Create(ProviderType.Subclass, (object)newItem.Value));
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException();
                    case NotifyCollectionChangedAction.Remove:
                        foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                            orderedProviders.Remove(Tuple.Create(ProviderType.Subclass, (object)oldItem.Value));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        throw new NotSupportedException();                        
                    case NotifyCollectionChangedAction.Reset:
                        foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                            orderedProviders.Remove(Tuple.Create(ProviderType.Subclass, (object)oldItem.Value));
                        break;
                }
            };
            return observedDictionary;            
        }

        private void ReplaceOrAddProvider(ProviderType type, object oldObj, object newObj) {
            var index = orderedProviders.IndexOf(Tuple.Create(type, oldObj));
            var newObjTuple = Tuple.Create(type, newObj);
            if (index > 0)
                orderedProviders[index] = newObjTuple;
            else
                orderedProviders.Add(newObjTuple);
        }        
    }
}
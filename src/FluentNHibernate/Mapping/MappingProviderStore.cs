using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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

        public IIdentityMappingProvider Id {
            get {
                return _id;
            }
            set {
                ReplaceOrAddSequencedProvider(_id, value);
                _id = value;
            }
        }

        public ICompositeIdMappingProvider CompositeId {
            get {
                return _compositeId;
            }
            set {
                ReplaceOrAddSequencedProvider(_compositeId, value);
                _compositeId = value;
            }
        }

        public INaturalIdMappingProvider NaturalId {
            get {
                return _naturalId;
            }
            set {
                ReplaceOrAddSequencedProvider(_naturalId, value);
                _naturalId = value;
            }
        }

        public IVersionMappingProvider Version {
            get {
                return _version;
            }
            set {
                ReplaceOrAddSequencedProvider(_version, value);
                _version = value;
            }
        }

        public IDiscriminatorMappingProvider Discriminator {
            get {
                return _discriminator;
            }
            set {
                ReplaceOrAddSequencedProvider(_discriminator, value);
                _discriminator = value;
            }
        }

        public TuplizerMapping TuplizerMapping {
            get {
                return _tuplizerMapping;
            }
            set {
                ReplaceOrAddSequencedProvider(_tuplizerMapping, value);
                _tuplizerMapping = value;
            }
        }

        private IIdentityMappingProvider _id;
        private ICompositeIdMappingProvider _compositeId;
        private INaturalIdMappingProvider _naturalId;
        private IVersionMappingProvider _version;
        private IDiscriminatorMappingProvider _discriminator;
        private TuplizerMapping _tuplizerMapping;

        private IList<object> sequencedMappingProviders;

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
            sequencedMappingProviders = new List<object>();
        }

        public IEnumerable<object> SequencedMappingProviders {
            get { return sequencedMappingProviders.Select(x => x); }
        }

        private IList<T> NewObservedList<T>() {
            var observableList = new ObservableCollection<T>();
            observableList.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        if (args.NewStartingIndex != ((IList<T>)sender).Count - 1)
                            throw new NotSupportedException(); // dont support insertions

                        foreach (T newItem in args.NewItems)
                            sequencedMappingProviders.Add(newItem);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException();

                    case NotifyCollectionChangedAction.Remove:
                        foreach (T oldItem in args.OldItems)
                            sequencedMappingProviders.Remove(oldItem);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        throw new NotSupportedException();

                    case NotifyCollectionChangedAction.Reset:
                        foreach (var oldItem in args.OldItems)
                            sequencedMappingProviders.Remove(oldItem);
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
                        if (args.NewStartingIndex != ((IDictionary<TKey, TVal>)sender).Count - 1)
                            throw new NotSupportedException(); // dont support insertions

                        foreach (KeyValuePair<TKey, TVal> newItem in args.NewItems)
                            sequencedMappingProviders.Add(newItem.Value);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException();

                    case NotifyCollectionChangedAction.Remove:
                        foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                            sequencedMappingProviders.Remove(oldItem.Value);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        throw new NotSupportedException();
                        
                    case NotifyCollectionChangedAction.Reset:
                        foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                            sequencedMappingProviders.Remove(oldItem.Value);
                        break;
                }
            };
            return observedDictionary;            
        }

        private void ReplaceOrAddSequencedProvider(object oldObj, object newObj) {
            var index = sequencedMappingProviders.IndexOf(oldObj);
            if (index > 0)
                sequencedMappingProviders[index] = newObj;
            else
                sequencedMappingProviders.Add(newObj);
        }        
    }
}
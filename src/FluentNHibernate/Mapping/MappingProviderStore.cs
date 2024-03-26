using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping;

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
    IIdentityMappingProvider _id;
    ICompositeIdMappingProvider _compositeId;
    INaturalIdMappingProvider _naturalId;
    IVersionMappingProvider _version;
    IDiscriminatorMappingProvider _discriminator;
    TuplizerMapping _tupilizerMapping;
    readonly IList<Tuple<ProviderType, object>> _orderedProviders;


    public IIdentityMappingProvider Id {
        get => _id;
        set {
            ReplaceOrAddProvider(ProviderType.Identity, _id, value);
            _id = value;
        }
    }

    public ICompositeIdMappingProvider CompositeId {
        get => _compositeId;
        set {
            ReplaceOrAddProvider(ProviderType.CompositeId, _compositeId, value);
            _compositeId = value;
        }
    }

    public INaturalIdMappingProvider NaturalId {
        get => _naturalId;
        set {
            ReplaceOrAddProvider(ProviderType.NaturalId, _naturalId, value);
            _naturalId = value;
        }
    }

    public IVersionMappingProvider Version {
        get => _version;
        set {
            ReplaceOrAddProvider(ProviderType.Version, _version, value);
            _version = value;
        }
    }

    public IDiscriminatorMappingProvider Discriminator {
        get => _discriminator;
        set {
            ReplaceOrAddProvider(ProviderType.Discriminator, _discriminator, value);
            _discriminator = value;
        }
    }

    public TuplizerMapping TuplizerMapping {
        get => _tupilizerMapping;
        set {
            ReplaceOrAddProvider(ProviderType.Tupilizer, _tupilizerMapping, value);
            _tupilizerMapping = value;
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
        _orderedProviders = new List<Tuple<ProviderType, object>>();
    }

    public IEnumerable<Tuple<ProviderType, object>> OrderedProviders {
        get { return _orderedProviders.Select(x => x); }
    }

    IList<T> NewObservedList<T>() {

        ProviderType TypeSelector(object mappingStoreCollection) 
        {
            if (ReferenceEquals(mappingStoreCollection, Properties)) {
                return ProviderType.Property;
            } else if (ReferenceEquals(mappingStoreCollection, Components)) {
                return ProviderType.Component;
            } else if (ReferenceEquals(mappingStoreCollection, OneToOnes)) {
                return ProviderType.OneToOne;
            } else if (ReferenceEquals(mappingStoreCollection, Collections)) {
                return ProviderType.Collection;
            } else if (ReferenceEquals(mappingStoreCollection, References)) {
                return ProviderType.ManyToOne;
            } else if (ReferenceEquals(mappingStoreCollection, Anys)) {
                return ProviderType.Any;
            } else if (ReferenceEquals(mappingStoreCollection, Filters)) {
                return ProviderType.Filter;
            } else if (ReferenceEquals(mappingStoreCollection, StoredProcedures)) {
                return ProviderType.StoredProcedure;
            } else if (ReferenceEquals(mappingStoreCollection, Joins)) {
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
                        _orderedProviders.Add(Tuple.Create(type, newItem));
                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotSupportedException();

                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in args.OldItems)
                        _orderedProviders.Remove(Tuple.Create(type, oldItem));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    foreach (var oldItem in args.OldItems)
                        _orderedProviders.Remove(Tuple.Create(type, oldItem));
                    break;
            }
        };
        return observableList;
    }

    IDictionary<TKey, TVal> NewObservedDictionary<TKey, TVal>() {
        var observedDictionary = new ObservableDictionary<TKey,TVal>();
        observedDictionary.CollectionChanged += (sender, args) => {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (args.NewStartingIndex < ((IDictionary<TKey, TVal>)sender).Count) {
                        //Inserting
                        for (var i = 0; i < args.NewItems.Count; i++) {
                            var newValue = (KeyValuePair<TKey, TVal>)args.NewItems[i];
                            _orderedProviders.Insert(args.NewStartingIndex + i, Tuple.Create(ProviderType.Subclass, (object)newValue.Value));
                        }
                    } else {
                        //Appending
                        foreach (KeyValuePair<TKey, TVal> newItem in args.NewItems) {
                            _orderedProviders.Add(Tuple.Create(ProviderType.Subclass, (object)newItem.Value));
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Remove:
                    foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                        _orderedProviders.Remove(Tuple.Create(ProviderType.Subclass, (object)oldItem.Value));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();                        
                case NotifyCollectionChangedAction.Reset:
                    foreach (KeyValuePair<TKey, TVal> oldItem in args.OldItems)
                        _orderedProviders.Remove(Tuple.Create(ProviderType.Subclass, (object)oldItem.Value));
                    break;
            }
        };
        return observedDictionary;            
    }

    void ReplaceOrAddProvider(ProviderType type, object oldObj, object newObj) {
        var index = _orderedProviders.IndexOf(Tuple.Create(type, oldObj));
        var newObjTuple = Tuple.Create(type, newObj);
        if (index > 0)
            _orderedProviders[index] = newObjTuple;
        else
            _orderedProviders.Add(newObjTuple);
    }        
}

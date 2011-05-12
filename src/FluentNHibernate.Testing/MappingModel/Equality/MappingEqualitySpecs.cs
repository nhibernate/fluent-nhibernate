using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Equality
{
    [TestFixture]
    public class when_comparing_two_identical_AnyMappings : MappingEqualitySpec<AnyMapping>
    {
        public override AnyMapping create_mapping()
        {
            var mapping = new AnyMapping
            {
                Access = "access",
                Cascade = "cascade",
                ContainingEntityType = typeof(Target),
                IdType = "id-type",
                Insert = true,
                Lazy = true,
                MetaType = new TypeReference(typeof(Target)),
                Name = "name",
                OptimisticLock = true,
                Update = true
            };

            mapping.AddIdentifierDefaultColumn(new ColumnMapping { Name = "default-id-col" });
            mapping.AddIdentifierColumn(new ColumnMapping { Name = "id-col" });
            mapping.AddMetaValue(new MetaValueMapping { Value = "value" });
            mapping.AddTypeDefaultColumn(new ColumnMapping { Name = "default-type-col" });
            mapping.AddTypeColumn(new ColumnMapping { Name = "type-col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_CollectionMappings : MappingEqualitySpec<CollectionMapping>
    {
        public override CollectionMapping create_mapping()
        {
            var mapping = CollectionMapping.Bag();

            mapping.Access = "access";
            mapping.Cascade = "cascade";
            mapping.ContainingEntityType = typeof(Target);
            mapping.Lazy = Lazy.True;
            mapping.Name = "name";
            mapping.OptimisticLock = true;
            mapping.BatchSize = 1;
            mapping.Cache = new CacheMapping();
            mapping.Check = "check";
            mapping.ChildType = typeof(Target);
            mapping.CollectionType = new TypeReference(typeof(Target));
            mapping.CompositeElement = new CompositeElementMapping();
            mapping.Element = new ElementMapping();
            mapping.Fetch = "fetch";
            mapping.Generic = true;
            mapping.Index = new IndexMapping();
            mapping.Inverse = true;
            mapping.Key = new KeyMapping();
            mapping.Member = new DummyPropertyInfo("name", typeof(Target)).ToMember();
            mapping.Mutable = true;
            mapping.OrderBy = "order-by";
            mapping.OtherSide = CollectionMapping.Bag();
            mapping.Persister = new TypeReference(typeof(Target));
            mapping.Relationship = new ManyToManyMapping();
            mapping.Schema = "schema";
            mapping.Subselect = "subselect";
            mapping.TableName = "table";
            mapping.Where = "where";
            mapping.AddFilter(new FilterMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_CacheMappings : MappingEqualitySpec<CacheMapping>
    {
        public override CacheMapping create_mapping()
        {
            return new CacheMapping
            {
                ContainedEntityType = typeof(Target),
                Include = "include",
                Region = "region",
                Usage = "usage"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ClassMappings : MappingEqualitySpec<ClassMapping>
    {
        public override ClassMapping create_mapping()
        {
            var mapping = new ClassMapping
            {
                Abstract = true, BatchSize = 10, Cache = new CacheMapping(),
                Check = "check", Discriminator = new DiscriminatorMapping(), DiscriminatorValue = "value",
                DynamicInsert = true, DynamicUpdate = true, EntityName = "entity-name",
                Id = new IdMapping(), Lazy = true, Mutable = true,
                Name = "name", OptimisticLock = "lock", Persister = "persister",
                Polymorphism = "poly", Proxy = "proxy", Schema = "schema",
                SchemaAction = "action", SelectBeforeUpdate = true, Subselect = "subselect",
                TableName = "table", Tuplizer = new TuplizerMapping(), Type = typeof(Target),
                Version = new VersionMapping(), Where = "where"
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_CompositeElementMappings : MappingEqualitySpec<CompositeElementMapping>
    {
        public override CompositeElementMapping create_mapping()
        {
            var mapping = new CompositeElementMapping
            {
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Parent = new ParentMapping(),
            };

            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_CompositeIdMappings : MappingEqualitySpec<CompositeIdMapping>
    {
        public override CompositeIdMapping create_mapping()
        {
            var mapping = new CompositeIdMapping
            {
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Access = "access",
                Mapped = true,
                Name = "name",
                UnsavedValue = "unsaved"
            };

            mapping.AddKey(new KeyManyToOneMapping());
            mapping.AddKey(new KeyPropertyMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ComponentMappings : MappingEqualitySpec<ComponentMapping>
    {
        public override ComponentMapping create_mapping()
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Access = "access",
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Insert = true,
                Lazy = true,
                Member = new DummyPropertyInfo("name", typeof(Target)).ToMember(),
                Name = "name",
                OptimisticLock = true,
                Parent = new ParentMapping(),
                Type = typeof(Target),
                Unique = true,
                Update = true
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ColumnMappings : MappingEqualitySpec<ColumnMapping>
    {
        public override ColumnMapping create_mapping()
        {
            return new ColumnMapping
            {
                Check = "check",
                Default = "default",
                Index = "index",
                Length = 1,
                Member = new DummyPropertyInfo("prop", typeof(Target)).ToMember(),
                Name = "name",
                NotNull = true,
                Precision = 1,
                Scale = 1,
                SqlType = "sql-type",
                Unique = true,
                UniqueKey = "unique-key"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_DiscriminatorMappings : MappingEqualitySpec<DiscriminatorMapping>
    {
        public override DiscriminatorMapping create_mapping()
        {
            var mapping = new DiscriminatorMapping
            {
                ContainingEntityType = typeof(Target),
                Force = true,
                Formula = "formula",
                Insert = true,
                Type = new TypeReference(typeof(Target))
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });
            
            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_DynamicComponentMappings : MappingEqualitySpec<ComponentMapping>
    {
        public override ComponentMapping create_mapping()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent)
            {
                Access = "access",
                ContainingEntityType = typeof(Target),
                Insert = true,
                Member = new DummyPropertyInfo("name", typeof(Target)).ToMember(),
                Name = "name",
                OptimisticLock = true,
                Parent = new ParentMapping(),
                Type = typeof(Target),
                Unique = true,
                Update = true
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ElementMappings : MappingEqualitySpec<ElementMapping>
    {
        public override ElementMapping create_mapping()
        {
            var mapping = new ElementMapping
            {
                ContainingEntityType = typeof(Target),
                Formula = "formula",
                Type = new TypeReference(typeof(Target))
            };

            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ExternalComponentMappings : MappingEqualitySpec<ExternalComponentMapping>
    {
        public override ExternalComponentMapping create_mapping()
        {
            var mapping = new ExternalComponentMapping(ComponentType.Component)
            {
                Access = "access",
                ContainingEntityType = typeof(Target),
                Insert = true,
                Member = new DummyPropertyInfo("name", typeof(Target)).ToMember(),
                Name = "name",
                OptimisticLock = true,
                Parent = new ParentMapping(),
                Type = typeof(Target),
                Unique = true,
                Update = true
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_FilterDefinitionMappings : MappingEqualitySpec<FilterDefinitionMapping>
    {
        public override FilterDefinitionMapping create_mapping()
        {
            var mapping = new FilterDefinitionMapping
            {
                Condition = "condition",
                Name = "name"
            };

            mapping.Parameters.Add("name", NHibernateUtil.Int32);

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_FilterMappings : MappingEqualitySpec<FilterMapping>
    {
        public override FilterMapping create_mapping()
        {
            var mapping = new FilterMapping
            {
                Condition = "condition",
                Name = "name"
            };

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_GeneratorMappings : MappingEqualitySpec<GeneratorMapping>
    {
        public override GeneratorMapping create_mapping()
        {
            var mapping = new GeneratorMapping
            {
                Class = "class",
                ContainingEntityType = typeof(Target),
            };

            mapping.Params.Add("left", "right");

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }


    [TestFixture]
    public class when_comparing_two_identical_HibernateMappings : MappingEqualitySpec<HibernateMapping>
    {
        public override HibernateMapping create_mapping()
        {
            var mapping = new HibernateMapping
            {
                Assembly = "assembly",
                AutoImport = true,
                Catalog = "catalog",
                DefaultAccess = "access",
                DefaultCascade = "cascade",
                DefaultLazy = true,
                Namespace = "namespace",
                Schema = "schema"
            };

            mapping.AddClass(new ClassMapping());
            mapping.AddFilter(new FilterDefinitionMapping());
            mapping.AddImport(new ImportMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_IdMappings : MappingEqualitySpec<IdMapping>
    {
        public override IdMapping create_mapping()
        {
            var mapping = new IdMapping
            {
                Member = new DummyPropertyInfo("prop", typeof(Target)).ToMember(),
                Name = "name",
                Access = "access",
                ContainingEntityType = typeof(Target),
                Generator = new GeneratorMapping(),
                Type = new TypeReference(typeof(Target)),
                UnsavedValue = "unsaved"
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ImportMappings : MappingEqualitySpec<ImportMapping>
    {
        public override ImportMapping create_mapping()
        {
            return new ImportMapping
            {
                Class = new TypeReference("class"),
                Rename = "rename"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_IndexManyToManyMappings : MappingEqualitySpec<IndexManyToManyMapping>
    {
        public override IndexManyToManyMapping create_mapping()
        {
            var mapping = new IndexManyToManyMapping
            {
                ContainingEntityType = typeof(Target),
                Class = new TypeReference(typeof(Target)),
                ForeignKey = "fk"
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_IndexMappings : MappingEqualitySpec<IndexMapping>
    {
        public override IndexMapping create_mapping()
        {
            var mapping = new IndexMapping
            {
                ContainingEntityType = typeof(Target),
                Type = new TypeReference(typeof(Target)),
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_JoinedSubclassMappings : MappingEqualitySpec<SubclassMapping>
    {
        public override SubclassMapping create_mapping()
        {
            var mapping = new SubclassMapping(SubclassType.JoinedSubclass)
            {
                Abstract = true,
                BatchSize = 10,
                Check = "check",
                DynamicInsert = true,
                DynamicUpdate = true,
                EntityName = "entity-name",
                Lazy = true,
                Name = "name",
                Persister = new TypeReference(typeof(Target)),
                Proxy = "proxy",
                Schema = "schema",
                SelectBeforeUpdate = true,
                Subselect = "subselect",
                TableName = "table",
                Type = typeof(Target),
                Extends = typeof(Target),
                Key = new KeyMapping()
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_JoinMappings : MappingEqualitySpec<JoinMapping>
    {
        public override JoinMapping create_mapping()
        {
            var mapping = new JoinMapping
            {
                Catalog = "catalog",
                ContainingEntityType = typeof(Target),
                Fetch = "fetch",
                Inverse = true,
                Key = new KeyMapping(),
                Optional = true,
                Schema = "schema",
                Subselect = "subselect",
                TableName = "table"
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_KeyManyToOneMappings : MappingEqualitySpec<KeyManyToOneMapping>
    {
        public override KeyManyToOneMapping create_mapping()
        {
            var mapping = new KeyManyToOneMapping
            {
                ContainingEntityType = typeof(Target),
                ForeignKey = "fk",
                Access = "access",
                Class = new TypeReference(typeof(Target)),
                Lazy = true,
                Name = "name",
                NotFound = "not-found"
            };

            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_KeyPropertyMappings : MappingEqualitySpec<KeyPropertyMapping>
    {
        public override KeyPropertyMapping create_mapping()
        {
            var mapping = new KeyPropertyMapping
            {
                ContainingEntityType = typeof(Target),
                Access = "access",
                Name = "name",
                Type = new TypeReference(typeof(Target))
            };

            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_KeyMappings : MappingEqualitySpec<KeyMapping>
    {
        public override KeyMapping create_mapping()
        {
            var mapping = new KeyMapping
            {
                ContainingEntityType = typeof(Target),
                ForeignKey = "fk",
                NotNull = true,
                OnDelete = "del",
                PropertyRef = "prop",
                Unique = true,
                Update = true
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ManyToOneMappings : MappingEqualitySpec<ManyToOneMapping>
    {
        public override ManyToOneMapping create_mapping()
        {
            var mapping = new ManyToOneMapping
            {
                Access = "access",
                Cascade = "cascade",
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Fetch = "fetch",
                ForeignKey = "fk",
                Insert = true,
                Lazy = Laziness.Proxy.ToString(),
                Member = new DummyPropertyInfo("prop", typeof(Target)).ToMember(),
                PropertyRef = "prop",
                Update = true,
                Name = "name",
                NotFound = "not found"
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ManyToManyMappings : MappingEqualitySpec<ManyToManyMapping>
    {
        public override ManyToManyMapping create_mapping()
        {
            var mapping = new ManyToManyMapping
            {
                ContainingEntityType = typeof(Target),
                Class = new TypeReference(typeof(Target)),
                ForeignKey = "fk",
                ChildType = typeof(Target),
                Fetch = "fetch",
                Lazy = true,
                NotFound = "not-found",
                ParentType = typeof(Target),
                Where = "where"
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_MetaValueMappings : MappingEqualitySpec<MetaValueMapping>
    {
        public override MetaValueMapping create_mapping()
        {
            return new MetaValueMapping
            {
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Value = "value"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_OneToManyMappings : MappingEqualitySpec<OneToManyMapping>
    {
        public override OneToManyMapping create_mapping()
        {
            return new OneToManyMapping
            {
                ContainingEntityType = typeof(Target),
                Class = new TypeReference(typeof(Target)),
                ChildType = typeof(Target),
                NotFound = "not-found"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_OneToOneMappings : MappingEqualitySpec<OneToOneMapping>
    {
        public override OneToOneMapping create_mapping()
        {
            return new OneToOneMapping
            {
                Access = "access",
                Cascade = "cascade",
                Class = new TypeReference(typeof(Target)),
                ContainingEntityType = typeof(Target),
                Fetch = "fetch",
                ForeignKey = "fk",
                Lazy = Laziness.Proxy.ToString(),
                PropertyRef = "prop",
                Name = "name",
                Constrained = true
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ParentMappings : MappingEqualitySpec<ParentMapping>
    {
        public override ParentMapping create_mapping()
        {
            return new ParentMapping
            {
                ContainingEntityType = typeof(Target),
                Name = "name"
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_PropertyMappings : MappingEqualitySpec<PropertyMapping>
    {
        public override PropertyMapping create_mapping()
        {
            var mapping = new PropertyMapping
            {
                Access = "access",
                ContainingEntityType = typeof(Target),
                Formula = "formula",
                Generated = "generated",
                Insert = true,
                Lazy = true,
                Member = new DummyPropertyInfo("prop", typeof(Target)).ToMember(),
                Name = "name",
                OptimisticLock = true,
                Type = new TypeReference(typeof(Target)),
                Update = true
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_ReferenceComponentMappings : MappingEqualitySpec<ReferenceComponentMapping>
    {
        public override ReferenceComponentMapping create_mapping()
        {
            var mapping = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("name", typeof(Target)).ToMember(), typeof(Target), typeof(Target), null);
            mapping.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));
            mapping.Access = "access";
            mapping.ContainingEntityType = typeof(Target);
            mapping.Insert = true;
            mapping.Name = "name";
            mapping.OptimisticLock = true;
            mapping.Parent = new ParentMapping();
            mapping.Unique = true;
            mapping.Update = true;
            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_StoredProcedureMappings : MappingEqualitySpec<StoredProcedureMapping>
    {
        public override StoredProcedureMapping create_mapping()
        {
            return new StoredProcedureMapping
            {
                Check = "check",
                Name = "name",
                Query = "query",
                SPType = "type",
                Type = typeof(Target)
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_SubclassMappings : MappingEqualitySpec<SubclassMapping>
    {
        public override SubclassMapping create_mapping()
        {
            var mapping = new SubclassMapping(SubclassType.Subclass)
            {
                Abstract = true,
                DynamicInsert = true,
                DynamicUpdate = true,
                EntityName = "entity-name",
                Lazy = true,
                Name = "name",
                Proxy = "proxy",
                SelectBeforeUpdate = true,
                Type = typeof(Target),
                Extends = typeof(Target),
                DiscriminatorValue = "value"
            };

            mapping.AddAny(new AnyMapping());
            mapping.AddCollection(CollectionMapping.Bag());
            mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            mapping.AddFilter(new FilterMapping());
            mapping.AddJoin(new JoinMapping());
            mapping.AddOneToOne(new OneToOneMapping());
            mapping.AddProperty(new PropertyMapping());
            mapping.AddReference(new ManyToOneMapping());
            mapping.AddStoredProcedure(new StoredProcedureMapping());
            mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_TuplizerMappings : MappingEqualitySpec<TuplizerMapping>
    {
        public override TuplizerMapping create_mapping()
        {
            return new TuplizerMapping
            {
                Mode = TuplizerMode.DynamicMap,
                Type = new TypeReference(typeof(Target))
            };
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_two_identical_VersionMappings : MappingEqualitySpec<VersionMapping>
    {
        public override VersionMapping create_mapping()
        {
            var mapping = new VersionMapping
            {
                Access = "access",
                ContainingEntityType = typeof(Target),
                Generated = "generated",
                Name = "name",
                Type = new TypeReference(typeof(Target)),
                UnsavedValue = "value"
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = "default" });
            mapping.AddColumn(new ColumnMapping { Name = "col" });

            return mapping;
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    public abstract class MappingEqualitySpec<T> : Specification
    {
        public abstract T create_mapping();

        public override void establish_context()
        {
            first_mapping = create_mapping();
            second_mapping = create_mapping();
        }

        public override void because()
        {
            are_equal = first_mapping.Equals(second_mapping);
        }

        protected T first_mapping;
        protected T second_mapping;
        protected bool are_equal;

        protected class Target {}
    }
}
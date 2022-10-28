using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
// ignoring warning for obsolete SubClass
#pragma warning disable 612,618
    [TestFixture]
    public class SubClassTester
    {
        [Test]
        public void CreateDiscriminator()
        {
            new MappingTester<SecondMappedObject>()
                .ForMapping(m =>
                {
				    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type");
                })
                .Element("class/discriminator").HasAttribute("type", "String")
                .Element("class/discriminator/column").HasAttribute("name", "Type");
        }

        [Test]
        public void CanUseDefaultDiscriminatorValue()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
				    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name));
                })
                .Element("class/subclass")
                    .DoesntHaveAttribute("discriminator-value");
        }

        [Test]
        public void ShouldSetTheName()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
			        m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", sc => { });
                })
                .Element("//subclass").HasAttribute("name", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void MapsProperty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name));
                })
                .Element("//subclass/property").HasAttribute("name", "Name");
        }

        [Test]
        public void SubClassLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.LazyLoad());
                })
                .Element("//subclass").HasAttribute("lazy", "true");
        }

        [Test]
        public void SubClassNotLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
			        m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Not.LazyLoad());
                })
                .Element("//subclass").HasAttribute("lazy", "false");
        }
        
        [Test]
        public void CanSpecifyProxyByType()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.Proxy(typeof(ProxyClass)));
              })
              .Element("class/subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyProxyByTypeInstance()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.Proxy<ProxyClass>());
              })
              .Element("class/subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyDynamicUpdate()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.DynamicUpdate());
              })
              .Element("class/subclass").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void CanSpecifyDynamicInsert()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.DynamicInsert());
              })
              .Element("class/subclass").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void CanSpecifySelectBeforeUpdate()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.SelectBeforeUpdate());
              })
              .Element("class/subclass").HasAttribute("select-before-update", "true");
        }

        [Test]
        public void CanSpecifyAbstract()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>("columnName", sm => sm.Abstract());
              })
              .Element("class/subclass").HasAttribute("abstract", "true");
        }

        [Test]
        public void CanSpecifySpecialNullValue()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>(DiscriminatorValue.Null, sm => { });
              })
              .Element("class/subclass").HasAttribute("discriminator-value", "null");
        }

        [Test]
        public void CanSpecifySpecialNotNullValue()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
              {
                  m.Id(x => x.Id);
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                      .SubClass<SecondMappedObject>(DiscriminatorValue.NotNull, sm => { });
              })
              .Element("class/subclass").HasAttribute("discriminator-value", "not null");
        }

        [Test]
        public void MapsComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.Component(x => x.Component, c => { }));
                })
                .Element("//subclass/component").Exists();
        }

        [Test]
        public void MapsDynamicComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.DynamicComponent(x => x.Dictionary, c => { }));
                })
                .Element("//subclass/dynamic-component").Exists();
        }

        [Test]
        public void MapsGenericDynamicComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.DynamicComponent(x => x.GenericDictionary, c => { }));
                })
                .Element("//subclass/dynamic-component").Exists();
        }

        [Test]
        public void MapsHasMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasMany(x => x.Children));
                })
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasManyToMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasManyToMany(x => x.Children));
                })
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasOne()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasOne(x => x.Parent));
                })
                .Element("//subclass/one-to-one").Exists();
        }

        [Test]
        public void MapsReferences()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.References(x => x.Parent));
                })
                .Element("//subclass/many-to-one").Exists();
        }

        [Test]
        public void MapsReferencesAny()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                            sc.ReferencesAny(x => x.Parent)
                                .IdentityType(x => x.Id)
                                .EntityIdentifierColumn("col")
                                .EntityTypeColumn("col"));
                })
                .Element("//subclass/any").Exists();
        }

        [Test]
        public void MapsSubSubclass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.SubClass<SecondMappedObject>(sc2 => { }));
                })
                .Element("//subclass/subclass").Exists();
        }

        [Test]
        public void SubSubclassIsLast()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                        {
                            sc.SubClass<SecondMappedObject>(sc2 => { });
                            sc.Map(x => x.Name);
                        });
                })
                .Element("//subclass/subclass").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void SubclassShouldNotHaveDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", sc =>
                        {
                            sc.Map(x => x.Name);
                            sc.SubClass<SecondMappedObject>("blue", sc2 => { });
                        });
                })
                 .Element("//subclass/discriminator").DoesntExist();
        }

        [Test]
        public void CreateDiscriminatorValueAtClassLevel()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type", "Foo")
                        .SubClass<SecondMappedObject>("Bar", sc => sc.Map(x => x.Name));
                })
                .Element("class")
                    .HasAttribute("discriminator-value", "Foo");
        }

        [Test]
        public void DiscriminatorAssumesStringIfNoTypeSupplied()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type");
                })
                .Element("class/discriminator")
                    .HasAttribute("type", "String");
        }

        [Test]
        public void CanSpecifyForceOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .AlwaysSelectWithValue();
                })
                .Element("class/discriminator")
                    .HasAttribute("force", "true");
        }

        [Test]
        public void CanSpecifyNotForcedOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Not.AlwaysSelectWithValue();
                })
                .Element("class/discriminator")
                    .HasAttribute("force", "false");
        }

        [Test]
        public void CanSpecifyReadOnlyOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .ReadOnly();
                })
                .Element("class/discriminator")
                    .HasAttribute("insert", "false");
        }

        [Test]
        public void CanSpecifyNotReadOnlyOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Not.ReadOnly();
                })
                .Element("class/discriminator")
                    .HasAttribute("insert", "true");
        }

        [Test]
        public void CanSpecifyNullableOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Nullable();
                })
                .Element("class/discriminator/column").HasAttribute("not-null", "false");
        }

        [Test]
        public void CanSpecifyNotNullableOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Not.Nullable();
                })
                .Element("class/discriminator/column").HasAttribute("not-null", "true");
        }

        [Test]
        public void CanSpecifyFormulaOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Formula("formula");
                })
                .Element("class/discriminator")
                    .HasAttribute("formula", "formula");
        }

        [Test]
        public void CanSpecifyLengthOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .Length(1234);
                })
                .Element("class/discriminator/column")
                    .HasAttribute("length", "1234");
        }

        [Test]
        public void MapsSubclassProperty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty));
                })
                .Element("//subclass/property")
                    .HasAttribute("name", "SubclassProperty");
        }


        [Test]
        public void SubclassPropertyHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestPropertyConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty));
                })
                .Element("//subclass/property[@name='SubclassProperty']")
                    .HasAttribute("generated", "never");
        }

        [Test]
        public void SubSubclassPropertyHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestPropertyConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty))
                        .SubClass<MappedObjectSubSubClass>("bar", sc => sc.Map(x => x.SubSubclassProperty));
                })
                .Element("//subclass/property[@name='SubSubclassProperty']")
                    .HasAttribute("generated", "never");

        }

        [Test]
        public void SubclassManyToOneReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestManyToOneConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.References(x => x.Child));
                })
                .Element("//subclass/many-to-one/column")
                    .HasAttribute("name", "test_column");
        }

        [Test]
        public void SubSubclassManyToOneReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestManyToOneConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc =>
                        {
                            sc.References(x => x.Child);
                            sc.SubClass<MappedObjectSubSubClass>("bar",
                                ssc => ssc.References(x => x.Child));
                        });
                })
                .Element("//subclass/subclass/many-to-one/column")
                    .HasAttribute("name", "test_column");

        }


        [Test]
        public void SubclassOneToManyReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestOneToManyConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.HasMany(x => x.Children).AsBag());
                })
                .Element("//subclass/bag/key")
                    .HasAttribute("foreign-key", "test_fk");

        }

        [Test]
        public void SubSubclassOneToManyReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => cf.Add<TestOneToManyConvention>())
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc =>
                        {
                            sc.HasMany(x => x.Children).AsBag();
                            sc.SubClass<MappedObjectSubSubClass>("bar", ssc => ssc.HasMany(x => x.Children).AsBag());
                        });
                })
                .Element("//subclass/subclass/bag/key")
                    .HasAttribute("foreign-key", "test_fk");

        }

        public class TestPropertyConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.Generated.Never();
            }
        }

        public class TestManyToOneConvention: IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column("test_column");
            }
        }

        public class TestOneToManyConvention: IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Key.Column("test_column");
                instance.Key.ForeignKey("test_fk");
            }
        }

        private class ProxyClass
        {}
    }

#pragma warning restore 612,618
}

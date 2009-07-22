using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class SubClassTester
    {
        [Test]
        public void CreateDiscriminator()
        {
            new MappingTester<SecondMappedObject>()
                .ForMapping(map => map.DiscriminateSubClassesOnColumn<string>("Type"))
                .Element("class/discriminator")
                    .Exists()
                    .HasAttribute("column", "Type")
                    .HasAttribute("type", "String");
        }

        [Test]
        public void CanUseDefaultDiscriminatorValue()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)))
                .Element("class/subclass")
                    .DoesntHaveAttribute("discriminator-value");
        }

        [Test]
        public void CreateTheSubClassMappings()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", sc => { }))
                .Element("//subclass")
                    .Exists()
                    .HasAttribute("name", typeof(SecondMappedObject).AssemblyQualifiedName)
                    .HasAttribute("discriminator-value", "red");
        }

        [Test]
        public void MapsProperty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)))
                .Element("//subclass/property").HasAttribute("name", "Name");
        }

        [Test]
        public void SubClassLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.LazyLoad()))
                .Element("//subclass").HasAttribute("lazy", "true");
        }

        [Test]
        public void SubClassNotLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Not.LazyLoad()))
                .Element("//subclass").HasAttribute("lazy", "false");
        }
        
        [Test]
        public void CanSpecifyProxyByType()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.Proxy(typeof(ProxyClass))))
              .Element("class/subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyProxyByTypeInstance()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.Proxy<ProxyClass>()))
              .Element("class/subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyDynamicUpdate()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.DynamicUpdate()))
              .Element("class/subclass").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void CanSpecifyDynamicInsert()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.DynamicInsert()))
              .Element("class/subclass").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void CanSpecifySelectBeforeUpdate()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.SelectBeforeUpdate()))
              .Element("class/subclass").HasAttribute("select-before-update", "true");
        }

        [Test]
        public void CanSpecifyAbstract()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("columnName", sm => sm.Abstract()))
              .Element("class/subclass").HasAttribute("abstract", "true");
        }

        [Test]
        public void CanSpecifySpecialNullValue()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(DiscriminatorValue.Null, sm => { }))
              .Element("class/subclass").HasAttribute("discriminator-value", "null");
        }

        [Test]
        public void CanSpecifySpecialNotNullValue()
        {
            new MappingTester<MappedObject>()
              .ForMapping(m =>
                  m.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(DiscriminatorValue.NotNull, sm => { }))
              .Element("class/subclass").HasAttribute("discriminator-value", "not null");
        }

        [Test]
        public void MapsComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.Component(x => x.Component, c => { })))
                .Element("//subclass/component").Exists();
        }

        [Test]
        public void MapsDynamicComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.DynamicComponent(x => x.Dictionary, c => { })))
                .Element("//subclass/dynamic-component").Exists();
        }

        [Test]
        public void MapsHasMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasMany(x => x.Children)))
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasManyToMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasManyToMany(x => x.Children)))
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasOne()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasOne(x => x.Parent)))
                .Element("//subclass/one-to-one").Exists();
        }

        [Test]
        public void MapsReferences()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.References(x => x.Parent)))
                .Element("//subclass/many-to-one").Exists();
        }

        [Test]
        public void MapsReferencesAny()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                            sc.ReferencesAny(x => x.Parent)
                                .IdentityType(x => x.Id)
                                .EntityIdentifierColumn("col")
                                .EntityTypeColumn("col")))
                .Element("//subclass/any").Exists();
        }

        [Test]
        public void MapsSubSubclass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.SubClass<SecondMappedObject>(sc2 => { })))
                .Element("//subclass/subclass").Exists();
        }

        [Test]
        public void SubSubclassIsLast()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                        {
                            sc.SubClass<SecondMappedObject>(sc2 => { });
                            sc.Map(x => x.Name);
                        }))
                .Element("//subclass/subclass").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void MapsVersion()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.Version(x => x.Version)))
                .Element("//subclass/version").Exists();
        }

        [Test]
        public void SubclassShouldNotHaveDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", m =>
                        {
                            m.Map(x => x.Name);
                            m.SubClass<SecondMappedObject>("blue", m2 =>
                            {});
                        }))
                 .Element("//subclass/discriminator").DoesntExist();
        }

        [Test]
        public void CreateDiscriminatorValueAtClassLevel()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type", "Foo")
                        .SubClass<SecondMappedObject>("Bar", m => m.Map(x => x.Name)))
                .Element("class")
                    .HasAttribute("discriminator-value", "Foo");
        }

        [Test]
        public void DiscriminatorAssumesStringIfNoTypeSupplied()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.DiscriminateSubClassesOnColumn("Type"))
                .Element("class/discriminator")
                    .HasAttribute("type", "String");
        }

        [Test]
        public void CanSpecifyForceOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .AlwaysSelectWithValue())
                .Element("class/discriminator")
                    .HasAttribute("force", "true");
        }

        [Test]
        public void CanSpecifyNotForcedOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .Not.AlwaysSelectWithValue())
                .Element("class/discriminator")
                    .HasAttribute("force", "false");
        }

        [Test]
        public void CanSpecifyReadOnlyOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .ReadOnly())
                .Element("class/discriminator")
                    .HasAttribute("insert", "false");
        }

        [Test]
        public void CanSpecifyNotReadOnlyOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .Not.ReadOnly())
                .Element("class/discriminator")
                    .HasAttribute("insert", "true");
        }

        [Test]
        public void CanSpecifyNullableOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .Nullable())
                .Element("class/discriminator")
                    .HasAttribute("not-null", "false");
        }

        [Test]
        public void CanSpecifyNotNullableOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .Not.Nullable())
                .Element("class/discriminator")
                    .HasAttribute("not-null", "true");
        }

        [Test]
        public void CanSpecifyFormulaOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .Formula("formula"))
                .Element("class/discriminator")
                    .HasAttribute("formula", "formula");
        }

        [Test]
        public void CanSpecifyLengthOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .WithLengthOf(1234))
                .Element("class/discriminator")
                    .HasAttribute("length", "1234");
        }

        [Test]
        public void CanSpecifyCustomAttributeOnDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SetAttribute("attr", "value"))
                .Element("class/discriminator")
                    .HasAttribute("attr", "value");
        }

        [Test]
        public void MapsSubclassProperty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty)))
                .Element("//subclass/property")
                    .HasAttribute("name", "SubclassProperty");
        }

        [Test]
        public void SubclassPropertyHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestPropertyConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty)))

                .Element("//subclass/property[@name='SubclassProperty']")
                    .HasAttribute("generated", "never");

        }

        [Test]
        public void SubSubclassPropertyHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestPropertyConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.Map(x => x.SubclassProperty))
                        .SubClass<MappedObjectSubSubClass>("bar", sc => sc.Map(x => x.SubSubclassProperty)))

                .Element("//subclass/property[@name='SubSubclassProperty']")
                    .HasAttribute("generated", "never");

        }

        [Test]
        public void SubclassManyToOneReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestManyToOneConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.References(x => x.Child)))

                .Element("//subclass/many-to-one")
                    .HasAttribute("column", "test_column");

        }

        [Test]
        public void SubSubclassManyToOneReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestManyToOneConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"),
                        sc => { 
                            sc.References(x => x.Child);
                            sc.SubClass<MappedObjectSubSubClass>("bar", 
                                ssc => ssc.References(x => x.Child));
                        }))
                .Element("//subclass/subclass/many-to-one")
                    .HasAttribute("column", "test_column");

        }


        [Test]
        public void SubclassOneToManyReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestOneToManyConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"), sc => sc.HasMany(x => x.Children).AsBag()))

                .Element("//subclass/bag/key")
                    .HasAttribute("foreign-key", "test_fk");

        }

        [Test]
        public void SubSubclassOneToManyReferenceHasConventionApplied()
        {
            new MappingTester<MappedObject>()
                .Conventions(cf => { cf.Add<TestOneToManyConvention>(); })
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type")
                        .SubClass<MappedObjectSubclass>(("foo"),
                        sc =>
                        {
                            sc.HasMany(x => x.Children).AsBag();
                            sc.SubClass<MappedObjectSubSubClass>("bar", ssc => ssc.HasMany(x => x.Children).AsBag());
                        }))
                .Element("//subclass/subclass/bag/key")
                    .HasAttribute("foreign-key", "test_fk");

        }

        public class TestPropertyConvention : IPropertyConvention
        {
            public bool Accept(IProperty target)
            {
                return true;
            }

            public void Apply(IProperty target)
            {
                target.SetAttribute("generated", "never");
            }
        }

        public class TestManyToOneConvention: IReferenceConvention
        {
            /// <summary>
            /// Whether this convention will be applied to the target.
            /// </summary>
            /// <param name="target">Instace that could be supplied</param>
            /// <returns>Apply on this target?</returns>
            public bool Accept(IManyToOnePart target)
            {
                return true;
            }

            /// <summary>
            /// Apply changes to the target
            /// </summary>
            /// <param name="target">Instance to apply changes to</param>
            public void Apply(IManyToOnePart target)
            {
                target.ColumnName("test_column");
            }
        }

        public class TestOneToManyConvention: IHasManyConvention
        {
            public bool Accept(IOneToManyPart target)
            {
                return true;
            }

            public void Apply(IOneToManyPart target)
            {
                target.KeyColumnNames.Add("test_column");
                target.WithForeignKeyConstraintName("test_fk");
            }
        }

        private class ProxyClass
        {}
    }
}
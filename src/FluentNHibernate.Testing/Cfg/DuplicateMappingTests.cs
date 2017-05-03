using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class DuplicateMappingTests
    {
        private Configuration cfg;
        private MappingConfiguration mapping;

        [SetUp]
        public void CreateMappingConfiguration()
        {
            cfg = new Configuration();

            SQLiteConfiguration.Standard
                .InMemory()
                .ConfigureProperties(cfg);

            mapping = new MappingConfiguration();
        }

        [Test]
        public void FluentMappingOfInheritedAutomappedTypeShouldBeIgnoredForSubclasses()
        {
            mapping.FluentMappings.Add(typeof(TypeInfoMap));
            mapping.FluentMappings.Add(typeof(MessageTypeMap));
            mapping.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(ActiveRecord))));
            mapping.Apply(cfg);

            var arMapping = cfg.GetClassMapping(typeof(ActiveRecord));
            arMapping.SubclassIterator.GetEnumerator().MoveNext().ShouldBeFalse();
            var messageTypeMapping = cfg.GetClassMapping(typeof(MessageType)) as SingleTableSubclass;
            messageTypeMapping.IsJoinedSubclass.ShouldBeFalse();
        }

        #region TestModel

        public class ActiveRecord
        {
            public virtual int Id { get; set; }
        }

        public class TypeInfo : ActiveRecord
        {
            public virtual int ClassId { get; set; }
        }

        public class MessageType : TypeInfo
        {
        }

        public class TypeInfoMap : ClassMap<TypeInfo>
        {
            public TypeInfoMap()
            {
                Table("TypeInfoNH");
                Id(t => t.Id).GeneratedBy.HiLo("10");
                Map(t => t.ClassId).ReadOnly();
                DiscriminateSubClassesOnColumn("ClassId", 102);
            }
        }

        public class MessageTypeMap : SubclassMap<MessageType>
        {
            public MessageTypeMap()
            {
                DiscriminatorValue(305);
            }
        }

        #endregion
    }
}

using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ManyToManySelfReferencedInverseIntegrationTester
    {
        public class TreeNode
        {
            public virtual int Id { get; set; }
            public virtual IEnumerable<TreeNode> Ancestors { get; set; }
            public virtual IEnumerable<TreeNode> Descendants { get; set; }
        }

        public class TreeNodeMap : ClassMap<TreeNode>
        {
            public TreeNodeMap()
            {
                Id(x => x.Id)
                    .GeneratedBy.Native();

                HasManyToMany(x => x.Ancestors)
                    .AsSet()
                    .Table("TreeNode_hierarhy")
                    .ParentKeyColumn("ChildID")
                    .ChildKeyColumn("ParentID")
                    .Cascade.SaveUpdate();

                HasManyToMany(x => x.Descendants)
                    .AsSet()
                    .Table("TreeNode_hierarhy")
                    .ParentKeyColumn("ParentID")
                    .ChildKeyColumn("ChildID")
                    .Inverse();
            }
        }

        private class ManyToManyPersistenceModel : PersistenceModel
        {
            public override void Configure(Configuration configuration)
            {
                Add(new TreeNodeMap());
                base.Configure(configuration);
            }
        }

        [Test]
        public void NHibernateCanLoadOneToManyTargetMapping()
        {
            var cfg = CreateStandardInMemoryConfiguration()
                .ConfigureProperties(new Configuration());

            var model = new ManyToManyPersistenceModel();
            model.Configure(cfg);

            cfg.BuildSessionFactory();
        }
    }
}
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;

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
            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ConfigureProperties(new Configuration());

            var model = new ManyToManyPersistenceModel();
            model.Configure(cfg);

            cfg.BuildSessionFactory();
        }
    }
}
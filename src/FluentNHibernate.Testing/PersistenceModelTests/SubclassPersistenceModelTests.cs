using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class SubclassPersistenceModelTests
    {
        [Test]
        public void ShouldPairSubclassWithClassMapOfSuperclass()
        {
            var model = new PersistenceModel();

            model.Add(new ParentClassMap());
            model.Add(new HierarchyOneChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Subclasses.Count().ShouldEqual(1);
            classMapping.Subclasses.First().Type.ShouldEqual(typeof(HierarchyOneChild));
        }

        [Test]
        public void ShouldPairSubclassOfSubclassWithParent()
        {
            var model = new PersistenceModel();

            model.Add(new ParentClassMap());
            model.Add(new HierarchyOneChildMap());
            model.Add(new HierarchyOneChildChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            // child
            classMapping.Subclasses.Count().ShouldEqual(1);
            var child = classMapping.Subclasses.First();

            child.Type.ShouldEqual(typeof(HierarchyOneChild));

            child.Subclasses.Count().ShouldEqual(1);
            child.Subclasses.First().Type.ShouldEqual(typeof(HierarchyOneChildChild));
        }

        [Test]
        public void ShouldPairSubclassOfSubclassWithParentWhenHasDiscriminator()
        {
            var model = new PersistenceModel();

            model.Add(new DiscriminatedParentClassMap());
            model.Add(new HierarchyTwoChildMap());
            model.Add(new HierarchyTwoChildChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            // child
            classMapping.Subclasses.Count().ShouldEqual(1);
            var child = classMapping.Subclasses.First();

            child.Type.ShouldEqual(typeof(HierarchyTwoChild));

            child.Subclasses.Count().ShouldEqual(1);
            child.Subclasses.First().Type.ShouldEqual(typeof(HierarchyTwoChildChild));
        }

        [Test]
        public void ShouldAddAsSubclassIfDiscriminatorExists()
        {
            var model = new PersistenceModel();

            model.Add(new DiscriminatedParentClassMap());
            model.Add(new HierarchyTwoChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            (classMapping.Subclasses.First() is SubclassMapping).ShouldBeTrue();
        }

        [Test]
        public void ShouldAddAsJoinedSubclassIfDiscriminatorDoesntExist()
        {
            var model = new PersistenceModel();

            model.Add(new ParentClassMap());
            model.Add(new HierarchyOneChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            (classMapping.Subclasses.First() is JoinedSubclassMapping).ShouldBeTrue();
        }

        [Test]
        public void AddByTypeShouldSupportSubclasses()
        {
            var model = new PersistenceModel();

            model.Add(new ParentClassMap());
            model.Add(typeof(HierarchyOneChildMap));

            model.BuildMappings()
                .First()
                .Classes.First()
                .Subclasses.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void ShouldPickUpSubclassMapsWhenAddingFromAssembly()
        {
            var model = new PersistenceModel();

            model.AddMappingsFromAssembly(typeof(HierarchyOneParent).Assembly);
            
            var classMapping = model.BuildMappings()
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(HierarchyOneParent)) != null)
                .Classes.First();

            classMapping.Subclasses.Count().ShouldBeGreaterThan(0);
        }
    }

    public class ParentClassMap : ClassMap<HierarchyOneParent>
    {
        public ParentClassMap()
        {
            Id(x => x.Id);
        }
    }

    public class DiscriminatedParentClassMap : ClassMap<HierarchyTwoParent>
    {
        public DiscriminatedParentClassMap()
        {
            Id(x => x.Id);
            DiscriminateSubClassesOnColumn("discriminator");
        }
    }

    public class HierarchyOneChildMap : SubclassMap<HierarchyOneChild>
    {}

    public class HierarchyOneChildChildMap : SubclassMap<HierarchyOneChildChild>
    {}

    public class HierarchyOneParent
    {
        public virtual int Id { get; set; }
    }

    public class HierarchyOneChild : HierarchyOneParent
    {

    }

    public class HierarchyOneChildChild : HierarchyOneChild
    {
        
    }

    public class HierarchyTwoChildMap : SubclassMap<HierarchyTwoChild>
    { }

    public class HierarchyTwoChildChildMap : SubclassMap<HierarchyTwoChildChild>
    { }

    public class HierarchyTwoParent
    {
        public virtual int Id { get; set; }
    }

    public class HierarchyTwoChild : HierarchyTwoParent
    {

    }

    public class HierarchyTwoChildChild : HierarchyTwoChild
    {

    }
}

using System;
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

            model.Add(new TablePerSubclass.TPS_ParentMap());
            model.Add(new TablePerSubclass.TPS_ChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Subclasses.Count().ShouldEqual(1);
            classMapping.Subclasses.First().Type.ShouldEqual(typeof(TablePerSubclass.TPS_Child));
        }

        [Test]
        public void ShouldPairSubclassOfSubclassWithParent()
        {
            var model = new PersistenceModel();

            model.Add(new TablePerSubclass.TPS_ParentMap());
            model.Add(new TablePerSubclass.TPS_ChildMap());
            model.Add(new TablePerSubclass.TPS_ChildChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            // child
            classMapping.Subclasses.Count().ShouldEqual(1);
            var child = classMapping.Subclasses.First();

            child.Type.ShouldEqual(typeof(TablePerSubclass.TPS_Child));

            child.Subclasses.Count().ShouldEqual(1);
            child.Subclasses.First().Type.ShouldEqual(typeof(TablePerSubclass.TPS_ChildChild));
        }

        [Test]
        public void ShouldPairSubclassOfSubclassWithParentWhenHasDiscriminator()
        {
            var model = new PersistenceModel();

            model.Add(new TablePerClassHierarchy.TPCH_ParentMap());
            model.Add(new TablePerClassHierarchy.TPCH_ChildMap());
            model.Add(new TablePerClassHierarchy.TPCH_ChildChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            // child
            classMapping.Subclasses.Count().ShouldEqual(1);
            var child = classMapping.Subclasses.First();

            child.Type.ShouldEqual(typeof(TablePerClassHierarchy.TPCH_Child));

            child.Subclasses.Count().ShouldEqual(1);
            child.Subclasses.First().Type.ShouldEqual(typeof(TablePerClassHierarchy.TPCH_ChildChild));
        }

        [Test]
        public void ShouldAddAsSubclassIfDiscriminatorExists()
        {
            var model = new PersistenceModel();

            model.Add(new TablePerClassHierarchy.TPCH_ParentMap());
            model.Add(new TablePerClassHierarchy.TPCH_ChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            (classMapping.Subclasses.First() is SubclassMapping).ShouldBeTrue();
        }

        [Test]
        public void ShouldAddAsJoinedSubclassIfDiscriminatorDoesntExist()
        {
            var model = new PersistenceModel();

            model.Add(new TablePerSubclass.TPS_ParentMap());
            model.Add(new TablePerSubclass.TPS_ChildMap());

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Subclasses.First().SubclassType.ShouldEqual(SubclassType.JoinedSubclass);
        }

        [Test]
        public void AddByTypeShouldSupportSubclasses()
        {
            var model = new PersistenceModel();

            model.Add(new TablePerSubclass.TPS_ParentMap());
            model.Add(typeof(TablePerSubclass.TPS_ChildMap));

            model.BuildMappings()
                .First()
                .Classes.First()
                .Subclasses.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void ShouldPickUpSubclassMapsWhenAddingFromAssembly()
        {
            var model = new PersistenceModel();

            model.AddMappingsFromSource(new StubTypeSource(typeof(TablePerSubclass.TPS_ParentMap), typeof(TablePerSubclass.TPS_ChildMap)));

            var classMapping = model.BuildMappings()
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(TablePerSubclass.TPS_Parent)) != null)
                .Classes.First();

            classMapping.Subclasses.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void ShouldPickupSubclassMapsWithIntermediaryClasses()
        {
            var model = new PersistenceModel();

            model.Add(new Intermediaries.I_TopMap());
            model.Add(new Intermediaries.I_BottomMostMap());

            model.BuildMappings()
                .First()
                .Classes.First()
                .Subclasses.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldHandleBranchingHierarchies()
        {
            var model = new PersistenceModel();

            model.Add(new Branching.B_TopMap());
            model.Add(new Branching.B_ChildMap());
            model.Add(new Branching.B_Child2Map());
            model.Add(new Branching.B_Child_ChildMap());
            model.Add(new Branching.B_Child2_ChildMap());

            var top = model.BuildMappings().First().Classes.First();

            top.Subclasses.ShouldContain(x => x.Type == typeof(Branching.B_Child));
            top.Subclasses.ShouldContain(x => x.Type == typeof(Branching.B_Child2));
            top.Subclasses.Count().ShouldEqual(2);

            var child = top.Subclasses.First(x => x.Type == typeof(Branching.B_Child));

            child.Subclasses.ShouldContain(x => x.Type == typeof(Branching.B_Child_Child));
            child.Subclasses.Count().ShouldEqual(1);

            var child2 = top.Subclasses.First(x => x.Type == typeof(Branching.B_Child2));

            child2.Subclasses.ShouldContain(x => x.Type == typeof(Branching.B_Child2_Child));
            child2.Subclasses.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldHandleInterfacesAsParents()
        {
            var model = new PersistenceModel();

            model.Add(new Interfaces.ITopMap());
            model.Add(new Interfaces.Int_OneMap());

            var top = model.BuildMappings().First().Classes.First();

            top.Type.ShouldEqual(typeof(Interfaces.ITop));
            top.Subclasses.ShouldContain(x => x.Type == typeof(Interfaces.Int_One));
            top.Subclasses.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldHandleEverettsWeirdMapping()
        {
            var model = new PersistenceModel();

            model.Add(new Sauces.SauceMap());
            model.Add(new Sauces.BrownSauceMap());
            model.Add(new Sauces.ReallyHotSauceMap());
            model.Add(new Thoughts.ThoughtMap());

            var colorSource = model.BuildMappings().First().Classes.First();

            colorSource.Type.ShouldEqual(typeof(Sauces.Sauce));
            colorSource.Subclasses.ShouldContain(x => x.Type == typeof(Sauces.BrownSauce));
            colorSource.Subclasses.ShouldContain(x => x.Type == typeof(Sauces.ReallyHotSauce));
            colorSource.Subclasses.Count().ShouldEqual(2);
        }

        [Test]
        public void CanBuildConfigurationForTablePerType()
        {
            var model = new PersistenceModel();
            model.Add(new TablePerType.TPT_TopMap());
            model.Add(new TablePerType.TPT_TopSubclassMap());
            model.Add(new TablePerType.TPT_MiddleMap());
            model.Add(new TablePerType.TPT_MiddleSubclassMap());

            var classMapping = model.BuildMappings();
            classMapping.Count().ShouldEqual(1);

            var top = classMapping.First().Classes.First();
            top.Subclasses.Count().ShouldEqual(2);

            var middle = top.Subclasses.SingleOrDefault(sc => sc.Type == typeof(TablePerType.TPT_Middle));
            middle.ShouldNotBeNull();
            middle.Subclasses.Count().ShouldEqual(1);
        }

        [Test]
        public void CanBuildConfigurationForTablePerTypeWithInterfaces()
        {
            var model = new PersistenceModel();
            model.Add(new TablePerTypeWithInterfaces.TPTWI_ITopMap());
            model.Add(new TablePerTypeWithInterfaces.TPTWI_TopSubclassMap());
            model.Add(new TablePerTypeWithInterfaces.TPTWI_IMiddleMap());
            model.Add(new TablePerTypeWithInterfaces.TPTWI_MiddleSubclassMap());

            var classMapping = model.BuildMappings();
            classMapping.Count().ShouldEqual(1);

            var top = classMapping.First().Classes.First();
            top.Subclasses.Count().ShouldEqual(2);

            var middle = top.Subclasses.SingleOrDefault(sc => sc.Type == typeof(TablePerTypeWithInterfaces.TPTWI_IMiddle));
            middle.ShouldNotBeNull();
            middle.Subclasses.Count().ShouldEqual(1);
        }
    }

    namespace Thoughts
    {
        public abstract class Thought
        {
            public virtual int Id { get; set; }
        }

        public class RandomThought : Thought
        { }

        public abstract class IntelligentThought : Thought
        { }

        public abstract class RevolutionaryThought : IntelligentThought
        { }

        public class Epiphany : RevolutionaryThought
        { }

        public sealed class ThoughtMap : ClassMap<Thought>
        {
            public ThoughtMap()
            {
                Id(x => x.Id);
#pragma warning disable 618,612
                JoinedSubClass<Epiphany>("Id", x => { });
                JoinedSubClass<RandomThought>("Id", x => { });
#pragma warning restore 618,612
            }
        }
    }

    namespace Interfaces
    {
        public interface ITop
        {
            int Id { get; set; }
        }

        public class Int_One : ITop
        {
            public virtual int Id { get; set; }
        }

        public class ITopMap : ClassMap<ITop>
        {
            public ITopMap()
            {
                Id(x => x.Id);
            }
        }

        public class Int_OneMap : SubclassMap<Int_One>
        {
            public Int_OneMap()
            {

            }
        }
    }

    namespace TablePerSubclass
    {
        public class TPS_Parent
        {
            public virtual int Id { get; set; }
        }

        public class TPS_Child : TPS_Parent
        {

        }

        public class TPS_ChildChild : TPS_Child
        {

        }

        public class TPS_ParentMap : ClassMap<TPS_Parent>
        {
            public TPS_ParentMap()
            {
                Id(x => x.Id);
            }
        }

        public class TPS_ChildMap : SubclassMap<TPS_Child>
        { }

        public class TPS_ChildChildMap : SubclassMap<TPS_ChildChild>
        { }
    }

    namespace TablePerClassHierarchy
    {
        public class TPCH_Parent
        {
            public virtual int Id { get; set; }
        }

        public class TPCH_Child : TPCH_Parent
        {

        }

        public class TPCH_ChildChild : TPCH_Child
        {

        }

        public class TPCH_ParentMap : ClassMap<TPCH_Parent>
        {
            public TPCH_ParentMap()
            {
                Id(x => x.Id);
                DiscriminateSubClassesOnColumn("discriminator");
            }
        }

        public class TPCH_ChildMap : SubclassMap<TPCH_Child>
        { }

        public class TPCH_ChildChildMap : SubclassMap<TPCH_ChildChild>
        { }
    }

    namespace Intermediaries
    {
        public class I_Top
        {
            public virtual int Id { get; set; }
        }

        public class I_Intermediary : I_Top
        {

        }

        public class I_Intermediary2 : I_Intermediary
        {

        }

        public class I_BottomMost : I_Intermediary2
        {

        }

        public class I_TopMap : ClassMap<I_Top>
        {
            public I_TopMap()
            {
                Id(x => x.Id);
            }
        }

        public class I_BottomMostMap : SubclassMap<I_BottomMost>
        { }
    }

    namespace TablePerType
    {
        public class TPT_Top
        {
            public virtual int Id { get; protected set; }
        }
        public class TPT_Middle
            : TPT_Top
        {
            public virtual string MiddleProperty { get; set; }
        }
        public class TPT_TopSubclass
            : TPT_Top
        {
        }
        public class TPT_MiddleSubclass
            : TPT_Middle
        {
            public virtual string OwnProperty { get; set; }
        }

        public class TPT_TopMap
            : ClassMap<TPT_Top>
        {
            public TPT_TopMap()
            {
                Id(x => x.Id);
            }
        }
        public class TPT_TopSubclassMap
            : SubclassMap<TPT_TopSubclass>
        {
            public TPT_TopSubclassMap()
            {
                KeyColumn("Id");
            }
        }
        public class TPT_MiddleMap
            : SubclassMap<TPT_Middle>
        {
            public TPT_MiddleMap()
            {
                KeyColumn("Id");
                Map(x => x.MiddleProperty);
            }
        }
        public class TPT_MiddleSubclassMap
            : SubclassMap<TPT_MiddleSubclass>
        {
            public TPT_MiddleSubclassMap()
            {
                KeyColumn("Id");
                Map(x => x.OwnProperty);
            }
        }
    }

    namespace TablePerTypeWithInterfaces
    {
        public interface TPTWI_ITop
        {
            int Id { get; }
        }
        public interface TPTWI_IMiddle
            : TPTWI_ITop
        {
            string MiddleProperty { get; set; }
        }

        public class TPTWI_TopSubclass
            : TPTWI_ITop
        {
            public virtual int Id { get; protected set; }
        }
        public class TPTWI_MiddleSubclass
            : TPTWI_TopSubclass, TPTWI_IMiddle
        {
            public virtual string MiddleProperty { get; set; }
            public virtual string OwnProperty { get; set; }
        }

        public class TPTWI_ITopMap
            : ClassMap<TPTWI_ITop>
        {
            public TPTWI_ITopMap()
            {
                Id(x => x.Id);
            }
        }
        public class TPTWI_TopSubclassMap
            : SubclassMap<TPTWI_TopSubclass>
        {
            public TPTWI_TopSubclassMap()
            {
                Extends<TPTWI_ITop>();
                KeyColumn("Id");
            }
        }
        public class TPTWI_IMiddleMap
            : SubclassMap<TPTWI_IMiddle>
        {
            public TPTWI_IMiddleMap()
            {
                KeyColumn("Id");
                Map(x => x.MiddleProperty);
            }
        }
        public class TPTWI_MiddleSubclassMap
            : SubclassMap<TPTWI_MiddleSubclass>
        {
            public TPTWI_MiddleSubclassMap()
            {
                KeyColumn("Id");
                Map(x => x.OwnProperty);
            }
        }
    }

    namespace Branching
    {
        public class B_Top
        {
            public virtual int Id { get; set; }
        }

        public class B_Child : B_Top
        {

        }

        public class B_Child2 : B_Top
        {

        }

        public class B_Child_Child : B_Child
        {

        }

        public class B_Child2_Child : B_Child2
        {

        }

        public class B_TopMap : ClassMap<B_Top>
        {
            public B_TopMap()
            {
                Id(x => x.Id);
            }
        }

        public class B_ChildMap : SubclassMap<B_Child>
        { }

        public class B_Child2Map : SubclassMap<B_Child2>
        { }

        public class B_Child_ChildMap : SubclassMap<B_Child_Child>
        { }

        public class B_Child2_ChildMap : SubclassMap<B_Child2_Child>
        { }
    }

    namespace Sauces
    {
        public abstract class Sauce
        {
            public virtual int Id { get; set; }
        }

        public class BrownSauce : Sauce
        { }

        public abstract class RedSauce : Sauce
        { }

        public abstract class HotSauce : RedSauce
        { }

        public class ReallyHotSauce : HotSauce
        { }

        public class SauceMap : ClassMap<Sauce>
        {
            public SauceMap()
            {
                Id(x => x.Id);
            }
        }

        public class ReallyHotSauceMap : SubclassMap<ReallyHotSauce>
        { }

        public class BrownSauceMap : SubclassMap<BrownSauce>
        { }
    }
}

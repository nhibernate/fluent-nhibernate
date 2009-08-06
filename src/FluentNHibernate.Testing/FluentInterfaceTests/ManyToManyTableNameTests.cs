using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManyTableNameTests
    {
        [Test]
        public void ShouldHaveSameTableNameForBothSidesOfMapping()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);
            rightMap.HasManyToMany(x => x.Lefts);

            model.Add(leftMap);
            model.Add(rightMap);

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();
            var rightMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Right)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("LeftsToRights");
            rightMapping.Collections.First().TableName.ShouldEqual("LeftsToRights");
        }

        [Test]
        public void ShouldHaveSameTableNameForBothSidesOfMappingWhenLeftSpecified()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights)
                .Table("MyJoinTable");

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);
            rightMap.HasManyToMany(x => x.Lefts);

            model.Add(leftMap);
            model.Add(rightMap);

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();
            var rightMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Right)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("MyJoinTable");
            rightMapping.Collections.First().TableName.ShouldEqual("MyJoinTable");
        }

        [Test]
        public void ShouldHaveSameTableNameForBothSidesOfMappingWhenRightSpecified()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);
            rightMap.HasManyToMany(x => x.Lefts)
                .Table("MyJoinTable");

            model.Add(leftMap);
            model.Add(rightMap);

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();
            var rightMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Right)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("MyJoinTable");
            rightMapping.Collections.First().TableName.ShouldEqual("MyJoinTable");
        }

        [Test]
        public void ShouldHaveSameTableNameForUniDirectionalMapping()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);

            model.Add(leftMap);
            model.Add(rightMap);

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("RightToLeft");
        }

        [Test]
        public void ShouldHaveSameTableNameForBothSidesOfMappingWhenHasMultipleBiDirectionalManyToManysOnSameEntities()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);
            leftMap.HasManyToMany(x => x.SecondRights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);
            rightMap.HasManyToMany(x => x.Lefts);
            rightMap.HasManyToMany(x => x.SecondLefts);

            model.Add(leftMap);
            model.Add(rightMap);

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();
            var rightMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Right)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("LeftsToRights");
            rightMapping.Collections.First().TableName.ShouldEqual("LeftsToRights");

            leftMapping.Collections.ElementAt(1).TableName.ShouldEqual("SecondLeftsToSecondRights");
            rightMapping.Collections.ElementAt(1).TableName.ShouldEqual("SecondLeftsToSecondRights");
        }

        [Test]
        public void ShouldAllowConventionsToAlterBiDirectionalTableNames()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);
            rightMap.HasManyToMany(x => x.Lefts);

            model.Add(leftMap);
            model.Add(rightMap);
            model.Conventions.Add<TestTableNameConvention>();

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();
            var rightMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Right)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("Lefts_Rights");
            rightMapping.Collections.First().TableName.ShouldEqual("Lefts_Rights");
        }

        [Test]
        public void ShouldAllowConventionsToAlterUniDirectionalTableNames()
        {
            var model = new PersistenceModel();
            var leftMap = new ClassMap<Left>();

            leftMap.Id(x => x.Id);
            leftMap.HasManyToMany(x => x.Rights);

            var rightMap = new ClassMap<Right>();

            rightMap.Id(x => x.Id);

            model.Add(leftMap);
            model.Add(rightMap);
            model.Conventions.Add<TestTableNameConvention>();

            var mappings = model.BuildMappings();

            var leftMapping = mappings.SelectMany(x => x.Classes).Where(x => x.Type == typeof(Left)).First();

            leftMapping.Collections.First().TableName.ShouldEqual("RightUni");
        }

        private class TestTableNameConvention : ManyToManyTableNameConvention
        {
            protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
            {
                return otherSide.Member.Name + "_" + collection.Member.Name;
            }

            protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
            {
                return collection.ChildType.Name + "Uni";
            }
        }
    }
}
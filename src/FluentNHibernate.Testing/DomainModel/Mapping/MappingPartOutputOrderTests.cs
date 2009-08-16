using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class MappingPartOutputOrderTests
    {
        [Test]
        public void PartsAreOrderedByPositionAndLevelRegardlessOfDeclaredOrder()
        {
            var mappingTester = new MappingTester<MappedObject>().ForMapping(mappedObject =>
            {
                mappedObject.HasMany(x => x.Children).AsBag();
                mappedObject.Version(x => x.Version);
                mappedObject.Id(x => x.Id);
            });

            mappingTester
                .Element("class/id").ShouldBeInParentAtPosition(0)
                .Element("class/version").ShouldBeInParentAtPosition(1)
                .Element("class/bag").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void PartsAreOrderedByPosition()
        {
            var mappingTester = new MappingTester<MappedObject>().ForMapping(mappedObject =>
            {
                mappedObject.DiscriminateSubClassesOnColumn<string>("Type").SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)); //Last
                mappedObject.Id(x => x.Id); //First
                mappedObject.HasMany(x => x.Children).AsBag(); //Anywhere

            });

            mappingTester
                .Element("class/id").ShouldBeInParentAtPosition(0)
                .Element("class/discriminator").ShouldBeInParentAtPosition(1) //created due to subclassing
                .Element("class/bag").ShouldBeInParentAtPosition(2)
                .Element("class/subclass").ShouldBeInParentAtPosition(3);
        }

        [Test]
        public void PartsWithSamePositionAreOrderedByLevel()
        {
            var mappingTester = new MappingTester<MappedObject>().ForMapping(mappedObject =>
            {
                mappedObject.Version(x => x.Version); //Level 4
                mappedObject.DiscriminateSubClassesOnColumn<string>("Type").SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)); //Level 3
                mappedObject.Id(x => x.Id); //Level 2
                mappedObject.Cache.ReadWrite(); //Level 1
            });

            mappingTester
                .Element("class/cache").ShouldBeInParentAtPosition(0)
                .Element("class/id").ShouldBeInParentAtPosition(1)
                .Element("class/discriminator").ShouldBeInParentAtPosition(2)
                .Element("class/version").ShouldBeInParentAtPosition(3);
        }

        [Test]
        public void PartsWithTheSameLevelAndPositionShouldRemainInTheOriginalAddedOrder()
        {

            var mappingTester = new MappingTester<MappedObject>().ForMapping(mappedObject =>
            {
                mappedObject.Id(x => x.Id);//First
                mappedObject.Map(x => x.NullableColor);//Anywhere
                mappedObject.Map(x => x.Version);//Anywhere
                mappedObject.Map(x => x.Id);//Anywhere
                mappedObject.Map(x => x.NickName);//Anywhere
                mappedObject.Map(x => x.Name);//Anywhere
                mappedObject.Map(x => x.Color);//Anywhere
                mappedObject.DiscriminateSubClassesOnColumn<string>("Type").SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)); //Last
            });

            mappingTester
                //Element 0 is id
                //Element 1 is discriminator
                .Element("class/property[@name='NullableColor']").ShouldBeInParentAtPosition(2)
                .Element("class/property[@name='Version']").ShouldBeInParentAtPosition(3)
                .Element("class/property[@name='Id']").ShouldBeInParentAtPosition(4)
                .Element("class/property[@name='NickName']").ShouldBeInParentAtPosition(5)
                .Element("class/property[@name='Name']").ShouldBeInParentAtPosition(6)
                .Element("class/property[@name='Color']").ShouldBeInParentAtPosition(7);
                //Element 8 is subclass
        }
    }
}
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Testing.Automapping.ManyToMany;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping;

[TestFixture]
public class AutoMapTests : BaseAutoMapFixture
{
    [Test]
    public void AutoMapIdentification()
    {
        Model<ExampleClass>();

        Test<ExampleClass>(mapping =>
        {
            mapping.Element("//id").HasAttribute("name", "Id");
            mapping.Element("//id//column").HasAttribute("name", "Id");
        });
    }

    [Test]
    public void AutoMapVersion()
    {
        Model<ExampleClass>();

        Test<ExampleClass>(mapping =>
            mapping
                .Element("//version").HasAttribute("name", "Timestamp")
                .Element("//version/column").HasAttribute("name", "Timestamp"));
    }

    [Test]
    public void AutoMapProperty()
    {
        Model<ExampleClass>();

        Test<ExampleClass>(mapping =>
            mapping.Element("//property[@name='LineOne']/column").HasAttribute("name", "LineOne"));
    }

    [Test]
    public void AutoMapIgnoreProperty()
    {
        Model<ExampleClass>(model => model
            .Override<ExampleClass>(m => m.IgnoreProperty(x => x.LineOne)));

        Test<ExampleClass>(mapping =>
            mapping.Element("//property[@name='LineOne']").DoesntExist());
    }

    [Test]
    public void ShouldAutoMapEnums()
    {
        Model<ExampleClass>(model => model
            .Override<ExampleClass>(mapping =>
                mapping.Map(x => x.Enum)));

        Test<ExampleClass>(mapping =>
            mapping.Element("//property[@name='Enum']").Exists());
    }

    [Test]
    public void AutoMapManyToOne()
    {
        Model<ExampleClass>();

        Test<ExampleClass>(mapping =>
            mapping.Element("//many-to-one").HasAttribute("name", "Parent")
                .Element("//many-to-one/column").HasAttribute("name", "Parent_id"));
    }

    [Test]
    public void AutoMapManyToMany()
    {
        Model<ManyToMany1>();

        Test<ManyToMany1>(mapping =>
            mapping.Element("//many-to-many/column").HasAttribute("name", "ManyToMany2_id"));
    }

    [Test]
    public void AutoMapManyToMany_ShouldRecognizeSet_BaseOnType()
    {
        Model<ManyToMany1>();

        Test<ManyToMany1>(mapping =>
            mapping.Element("//set[@name='Many1']").Exists());
    }

    [Test]
    public void AutoMapOneToMany()
    {
        Model<ExampleParentClass>();

        Test<ExampleParentClass>(mapping =>
            mapping.Element("//bag[@name='Examples']").Exists());
    }

    [Test]
    public void AutoMapDoesntSetCacheWithDefaultConvention()
    {
        Model<ExampleClass>();

        Test<ExampleClass>(mapping =>
            mapping.Element("//cache").DoesntExist());
    }

    [Test]
    public void AutoMapSetsCacheOnClassUsingConvention()
    {
        Model<ExampleClass>(model => model
            .Conventions.Add(Cache.Is(cache => cache.ReadOnly())));

        Test<ExampleClass>(mapping =>
            mapping.Element("//cache").Exists());
    }

    [Test]
    public void ShouldAutoMapBitmapPropertyAsColumn()
    {
        //This is a regression test for issue 198
        Model<ClassWithBitmap>();

        Test<ClassWithBitmap>(mapping =>
            mapping.Element("//property[@name='Bitmap']/column").HasAttribute("name", "Bitmap"));
    }

    [Test]
    public void ShouldAutoMapInheritedIdAutoPropertyWithPrivateSetter()
    {
        Model<ClassThatInheritsIdFromParentWithPrivateSetter>();

        Test<ClassThatInheritsIdFromParentWithPrivateSetter>(mapping =>
            mapping.Element("//id").HasAttribute("access", "backfield"));
    }

}

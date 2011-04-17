using NUnit.Framework;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class AttributeStoreTester
    {
        private sealed class TestStore : AttributeStore
        {
            public bool IsSomething
            {
                get { return this.GetOrDefault<bool>("IsSomething"); }
            }

            public string Name
            {
                get { return this.GetOrDefault<string>("Name"); }
            }
        }

        [Test]
        public void UnsetAttributeShouldBeDefault()
        {
            var store = new TestStore();
            store.IsSomething.ShouldBeFalse();
        }

        [Test]
        public void CanGetAndSetAttribute()
        {
            var store = new TestStore();
            store.Set("IsSomething", Layer.Defaults, true);
            store.IsSomething.ShouldBeTrue();            
        }

        [Test]
        public void CanCheckIfAttributeIsSpecified()
        {

            var store = new TestStore();            
            store.IsSpecified("IsSomething").ShouldBeFalse();
            store.Set("IsSomething", Layer.Defaults, true);
            store.IsSpecified("IsSomething").ShouldBeTrue();
        }

        [Test]
        public void CanCopyAttributes()
        {
            var source = new TestStore();
            source.Set("IsSomething", Layer.Defaults, true);

            var target = new TestStore();
            source.CopyTo(target);

            target.IsSomething.ShouldBeTrue();
        }

        [Test]
        public void CopyingAttributesReplacesOldValues()
        {
            var source = new TestStore();
            source.Set("IsSomething", Layer.Defaults, false);

            var target = new TestStore();
            target.Set("IsSomething", Layer.Defaults, true);
            source.CopyTo(target);

            target.IsSomething.ShouldBeFalse();
        }

        [Test]
        public void UnsetValuesAreNotCopied()
        {
            var source = new TestStore();
            var target = new TestStore();
            target.Set("IsSomething", Layer.Defaults, true);
            source.CopyTo(target);

            target.IsSomething.ShouldBeTrue();
        }

        [Test]
        public void CanSetDefaultValue()
        {
            var source = new TestStore();
            source.Set("IsSomething", Layer.Defaults, true);
            
            source.IsSomething.ShouldBeTrue();
        }
    }
}
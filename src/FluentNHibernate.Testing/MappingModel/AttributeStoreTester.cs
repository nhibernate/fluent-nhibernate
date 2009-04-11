using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using System.Collections.Generic;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class AttributeStoreTester
    {
        private sealed class TestStore : AttributeStore<TestStore>
        {
            public TestStore() : base(new AttributeStore())
            {
            }

            public bool IsSomething
            {
                get { return Get(x => x.IsSomething); }
                set { Set(x => x.IsSomething, value); }
            }

            public string Name
            {
                get { return Get(x => x.Name); }
                set { Set(x => x.Name, value); }
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
            store.IsSomething = true;
            store.IsSomething.ShouldBeTrue();            
        }

        [Test]
        public void CanCheckIfAttributeIsSpecified()
        {

            var store = new TestStore();            
            store.IsSpecified(x => x.IsSomething).ShouldBeFalse();
            store.IsSomething = true;
            store.IsSpecified(x => x.IsSomething).ShouldBeTrue();
        }

        [Test]
        public void CanCopyAttributes()
        {
            var source = new TestStore();
            source.IsSomething = true;

            var target = new TestStore();
            source.CopyTo(target);

            target.IsSomething.ShouldBeTrue();
        }

        [Test]
        public void CopyingAttributesReplacesOldValues()
        {
            var source = new TestStore();
            source.IsSomething = false;

            var target = new TestStore();
            target.IsSomething = true;
            source.CopyTo(target);

            target.IsSomething.ShouldBeFalse();
        }

        [Test]
        public void UnsetValuesAreNotCopied()
        {
            var source = new TestStore();

            var target = new TestStore();
            target.IsSomething = true;
            source.CopyTo(target);

            target.IsSomething.ShouldBeTrue();
        }

        [Test]
        public void CanSetDefaultValue()
        {
            var source = new TestStore();
            source.SetDefault(x => x.IsSomething, true);
            
            source.IsSomething.ShouldBeTrue();
        }

        [Test]
        public void DefaultValuesAreNotCopied()
        {
            var source = new TestStore();
            source.SetDefault(x => x.IsSomething, true);

            var target = new TestStore();
            target.IsSomething = false;
            source.CopyTo(target);

            target.IsSomething.ShouldBeFalse();
        }
    }
}
using System;
using System.Collections.Generic;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class MemberBackingFieldTests
    {
        [Test]
        public void PropertyWithCamelCaseField()
        {
            var member = ReflectionHelper.GetMember<Example, string>(x => x.PropertyWithCamelCaseField);
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("propertyWithCamelCaseField");
        }

        [Test]
        public void PropertyWithLowerCaseField()
        {
            var member = ReflectionHelper.GetMember<Example, string>(x => x.PropertyWithLowerCaseField);
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("propertywithlowercasefield");
        }

        [Test]
        public void PropertyWithUnderscoreCamelCaseField()
        {
            var member = ReflectionHelper.GetMember<Example, string>(x => x.PropertyWithUnderscoreCamelCaseField);
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("_propertyWithUnderscoreCamelCaseField");
        }

        [Test]
        public void PropertyWithUnderscorePascalCaseField()
        {
            var member = ReflectionHelper.GetMember<Example, string>(x => x.PropertyWithUnderscorePascalCaseField);
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("_PropertyWithUnderscorePascalCaseField");
        }

        [Test]
        public void MethodWithCamelCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.CamelCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("camelCaseMethod");
        }

        [Test]
        public void MethodWithLowerCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.LowerCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("lowercasemethod");
        }

        [Test]
        public void MethodWithUnderscorePascalCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.UnderscorePascalCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("_UnderscorePascalCaseMethod");
        }

        [Test]
        public void GetMethodWithCamelCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.GetCamelCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("camelCaseMethod");
        }

        [Test]
        public void GetMethodWithLowerCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.GetLowerCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("lowercasemethod");
        }

        [Test]
        public void GetMethodWithUnderscorePascalCaseBackingField()
        {
            var member = ReflectionHelper.GetMember<Example, IEnumerable<Example>>(x => x.GetUnderscorePascalCaseMethod());
            Member field;

            member.TryGetBackingField(out field);

            field.IsField.ShouldBeTrue();
            field.Name.ShouldEqual("_UnderscorePascalCaseMethod");
        }

#pragma warning disable 649
        class Example
        {
            string propertyWithCamelCaseField;
            string _propertyWithUnderscoreCamelCaseField;
            string _PropertyWithUnderscorePascalCaseField;
            string propertywithlowercasefield;
            IEnumerable<Example> camelCaseMethod;
            IEnumerable<Example> lowercasemethod;
            IEnumerable<Example> _UnderscorePascalCaseMethod;

            public string PropertyWithCamelCaseField
            {
                get { return propertyWithCamelCaseField; }
            }

            public string PropertyWithLowerCaseField
            {
                get { return propertywithlowercasefield; }
            }

            public string PropertyWithUnderscoreCamelCaseField
            {
                get { return _propertyWithUnderscoreCamelCaseField; }
            }

            public string PropertyWithUnderscorePascalCaseField
            {
                get { return _PropertyWithUnderscorePascalCaseField; }
            }

            public IEnumerable<Example> CamelCaseMethod()
            {
                return camelCaseMethod;
            }

            public IEnumerable<Example> LowerCaseMethod()
            {
                return lowercasemethod;
            }

            public IEnumerable<Example> UnderscorePascalCaseMethod()
            {
                return _UnderscorePascalCaseMethod;
            }

            public IEnumerable<Example> GetCamelCaseMethod()
            {
                return camelCaseMethod;
            }

            public IEnumerable<Example> GetLowerCaseMethod()
            {
                return lowercasemethod;
            }

            public IEnumerable<Example> GetUnderscorePascalCaseMethod()
            {
                return _UnderscorePascalCaseMethod;
            }
        }
#pragma warning restore 649
    }
}
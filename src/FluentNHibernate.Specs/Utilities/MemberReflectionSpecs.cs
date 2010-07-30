using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Utilities
{
    public class when_getting_all_members_for_a_class
    {
        Because of = () =>
            members = typeof(MyClass).GetInstanceMembers();

        It should_include_the_public_properties = () =>
            members.Select(x => x.Name).ShouldContain("PublicProperty");

        It should_include_the_private_properties = () =>
            members.Select(x => x.Name).ShouldContain("PrivateProperty");

        It should_include_the_protected_properties = () =>
            members.Select(x => x.Name).ShouldContain("ProtectedProperty");

        It should_include_the_public_fields = () =>
            members.Select(x => x.Name).ShouldContain("publicField");

        It should_include_the_private_fields = () =>
            members.Select(x => x.Name).ShouldContain("privateField");

        It should_include_the_protected_fields = () =>
            members.Select(x => x.Name).ShouldContain("protectedField");
        
        It should_not_include_backing_fields = () =>
            members.Select(x => x.Name).ShouldNotContain("<PublicProperty>k__BackingField");

        It should_not_include_property_methods = () =>
            members.Select(x => x.Name).ShouldNotContain("get_PublicProperty", "set_PublicProperty");
        
        It should_include_the_public_parameterless_non_void_methods = () =>
            members.Select(x => x.Name).ShouldContain("PublicNonVoidParameterlessMethod");

        It should_not_include_the_public_non_void_methods_with_parameters = () =>
            members.Select(x => x.Name).ShouldNotContain("PublicNonVoidMethodWithParameters");

        It should_not_include_the_public_parameterless_void_methods = () =>
            members.Select(x => x.Name).ShouldNotContain("PublicVoidParameterlessMethod");

        It should_not_include_the_public_void_methods_with_parameters = () =>
            members.Select(x => x.Name).ShouldNotContain("PublicVoidMethodWithParameters");

        It should_include_the_private_parameterless_non_void_methods = () =>
            members.Select(x => x.Name).ShouldContain("PrivateNonVoidParameterlessMethod");

        It should_not_include_the_private_non_void_methods_with_parameters = () =>
            members.Select(x => x.Name).ShouldNotContain("PrivateNonVoidMethodWithParameters");

        It should_not_include_the_private_parameterless_void_methods = () =>
            members.Select(x => x.Name).ShouldNotContain("PrivateVoidParameterlessMethod");

        It should_not_include_the_private_void_methods_with_parameters = () =>
            members.Select(x => x.Name).ShouldNotContain("PrivateVoidMethodWithParameters");
        
        static IEnumerable<Member> members;

        class MyClass
        {
            public int PublicProperty { get; set; }
            protected int ProtectedProperty { get; set; }
            int PrivateProperty { get; set; }

            public int publicField;
            protected int protectedField;
            int privateField;

            public int PublicNonVoidParameterlessMethod()
            {
                return 0;
            }

            public int PublicNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            public void PublicVoidParameterlessMethod() {}
            public void PublicVoidMethodWithParameters(int param) { }

            int PrivateNonVoidParameterlessMethod()
            {
                return 0;
            }

            int PrivateNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            void PrivateVoidParameterlessMethod() { }
            void PrivateVoidMethodWithParameters(int param) { }

            public int ProtectedNonVoidParameterlessMethod()
            {
                return 0;
            }

            public int ProtectedNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            public void ProtectedVoidParameterlessMethod() { }
            public void ProtectedVoidMethodWithParameters(int param) { }
        }
    }

    public class when_getting_all_members_for_a_descendent_class
    {
        Because of = () =>
            members = typeof(MyClass).GetInstanceMembers();

        It should_include_the_public_properties_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("PublicProperty");

        It should_include_the_private_properties_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("PrivateProperty");

        It should_include_the_protected_properties_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("ProtectedProperty");

        It should_include_the_public_fields_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("publicField");

        It should_include_the_private_fields_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("privateField");

        It should_include_the_protected_fields_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("protectedField");

        It should_include_the_public_parameterless_non_void_methods_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("PublicNonVoidParameterlessMethod");

        It should_include_the_private_parameterless_non_void_methods_of_the_parent = () =>
            members.Select(x => x.Name).ShouldContain("PrivateNonVoidParameterlessMethod");

        static IEnumerable<Member> members;

        class MyClass : Base
        {}

        class Base
        {
            public int PublicProperty { get; set; }
            protected int ProtectedProperty { get; set; }
            int PrivateProperty { get; set; }

            public int publicField;
            protected int protectedField;
            int privateField;

            public int PublicNonVoidParameterlessMethod()
            {
                return 0;
            }

            public int PublicNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            public void PublicVoidParameterlessMethod() { }
            public void PublicVoidMethodWithParameters(int param) { }

            int PrivateNonVoidParameterlessMethod()
            {
                return 0;
            }

            int PrivateNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            void PrivateVoidParameterlessMethod() { }
            void PrivateVoidMethodWithParameters(int param) { }

            public int ProtectedNonVoidParameterlessMethod()
            {
                return 0;
            }

            public int ProtectedNonVoidMethodWithParameters(int param)
            {
                return 0;
            }

            public void ProtectedVoidParameterlessMethod() { }
            public void ProtectedVoidMethodWithParameters(int param) { }
        }
    }

}
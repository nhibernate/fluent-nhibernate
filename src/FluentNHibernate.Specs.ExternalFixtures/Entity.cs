using System.Collections.Generic;

namespace FluentNHibernate.Specs.ExternalFixtures
{
    public class Entity
    {
        EntityChild readOnlyChild;

        public int Id { get; set; }
        public string One { get; set; }
        public TestEnum Enum { get; set; }
        public Entity Parent { get; set; }
        public IList<EntityChild> Children { get; set; }
        public Component Component { get; set; }

        public enum TestEnum {}

        public EntityChild ReadOnlyChild
        {
            get { return readOnlyChild; }
        }
    }

    public class EntityChild
    {}

    public abstract class A_Child : B_Parent
    {
        
    }

    public class B_Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Child : Parent
    {

    }

    public class Parent
    {
        public int Id { get; set; }
        public Component Component { get; set; }
    }

    public class ChildChild : Child
    {

    }
}
using System.Collections.Generic;
using System;
using Iesi.Collections.Generic;

namespace FluentNHibernate.AutoMap.TestFixtures
{
    public class ExampleCustomColumn
    {
        public int Id { get; set; }
        public int ExampleCustomColumnId { get; set; }
        public int CustomColumn
        {
            get
            {
                return 12;
            }
        }
    }

/*
    public class ExampleInheritedClass : ExampleClass
    {
        public int Id { get; set; }
        public int ExampleInheritedClassId { get; set; } 
        public string ExampleProperty { get; set; }
    }
*/

    public class ExampleClass
    {
        public virtual int Id { get; set; }
        public virtual int ExampleClassId { get; set; }
        public virtual string LineOne { get; set; }
        public DateTime Timestamp { get; set; }
        public ExampleEnum Enum { get; set; }
        public ExampleParentClass Parent { get; set; }
    }


    public enum ExampleEnum
    {
        enum1=1
    }

    public class ExampleParentClass
    {
        public int ExampleParentClassId { get; set; } 
        public virtual int Id { get; set; }
        public virtual IList<ExampleClass> Examples {get; set;}
    }
}

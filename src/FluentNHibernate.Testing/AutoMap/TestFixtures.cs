using System.Collections.Generic;
using System;

namespace FluentNHibernate.AutoMap.TestFixtures
{
    public class ExampleCustomColumn
    {
        public int Id { get; set; }
        public int CustomColumn
        {
            get
            {
                return 12;
            }
        }
    }

    public class ExampleInheritedClass : ExampleClass
    {
        public int Id { get; set; } 
        public string ExampleProperty { get; set; }
    }

    public class ExampleClass
    {
        public virtual int Id { get; set; }
        public virtual string LineOne { get; set; }
        public DateTime Timestamp { get; set; }
        public ExampleParentClass Parent { get; set; }
    }

    public class ExampleParentClass
    {
        public virtual int Id { get; set; }
        public virtual IList<ExampleClass> Examples {get; set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping
{
    [TestFixture]
    public class InheritanceTests: BaseAutoMapFixture
    {
        [Test]
        public void AutoMapSimpleProperties()
        {
            Model<ThirdLevel>();

            Test<ThirdLevel>(mapping =>
            {
                mapping.Element("//id").HasAttribute("name", "Id");
                mapping.Element("//property[@name='Rate']/column").HasAttribute("name", "Rate");
                mapping.Element("//property[@name='SecondRate']/column").HasAttribute("name", "SecondRate");
            });
        }
    }

    public abstract class Base1
    {
        public virtual int Id { get; protected set; }
        public abstract void Foo(int x);
    }

    public class Derived1: Base1
    {
        public virtual Decimal Rate { get; set; }
        public override void Foo(int x)
        {
        }

        public string GetSomething()
        {
            return Environment.NewLine;
        }
    }

    public class SecondLevel: Derived1
    {
        public virtual Double SecondRate { get; set; }
    }

    public class ThirdLevel: SecondLevel
    {
        public virtual Boolean Flag { get; set; }
    }
}

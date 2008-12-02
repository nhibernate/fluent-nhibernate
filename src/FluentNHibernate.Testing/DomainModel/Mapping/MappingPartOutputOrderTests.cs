using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class MappingPartOutputOrderTests
    {
        [Test]
        public void PartsAreOrderedByLevelRegardlessOfInsertionOrder()
        {
            var elements = new List<IMappingPart>();

            var one = new StubbedMappingPart(1);
            var two = new StubbedMappingPart(2);
            var three = new StubbedMappingPart(3);
            var four = new StubbedMappingPart(4);

            elements.AddRange(new [] { four, two, one, three });
            elements.Sort(new MappingPartComparer());

            Assert.AreEqual(one, elements[0]);
            Assert.AreEqual(two, elements[1]);
            Assert.AreEqual(three, elements[2]);
            Assert.AreEqual(four, elements[3]);
        }

        [Test]
        public void PartsAreOrderedByPosition()
        {
            var elements = new List<IMappingPart>();

            var first = new StubbedMappingPart(PartPosition.First);
            var anywhere = new StubbedMappingPart(PartPosition.Anywhere);
            var last = new StubbedMappingPart(PartPosition.Last);

            elements.AddRange(new[] { last, first, anywhere });
            elements.Sort(new MappingPartComparer());

            Assert.AreEqual(first, elements[0]);
            Assert.AreEqual(anywhere, elements[1]);
            Assert.AreEqual(last, elements[2]);
        }

        [Test]
        public void PartsWithSamePositionAreOrderedByLevel()
        {
            var elements = new List<IMappingPart>();

            var one = new StubbedMappingPart(1, PartPosition.Anywhere);
            var two = new StubbedMappingPart(2, PartPosition.Anywhere);
            var three = new StubbedMappingPart(3, PartPosition.Anywhere);
            var four = new StubbedMappingPart(4, PartPosition.Anywhere);

            elements.AddRange(new[] { four, two, one, three });
            elements.Sort(new MappingPartComparer());

            Assert.AreEqual(one, elements[0]);
            Assert.AreEqual(two, elements[1]);
            Assert.AreEqual(three, elements[2]);
            Assert.AreEqual(four, elements[3]);
        }

        [Test]
        public void FirstComesFirstLastComesLastAnywheresInTheMiddleOrderedByLevel()
        {
            var elements = new List<IMappingPart>();

            var first = new StubbedMappingPart(1, PartPosition.First);
            var one = new StubbedMappingPart(2, PartPosition.Anywhere);
            var two = new StubbedMappingPart(3, PartPosition.Anywhere);
            var last = new StubbedMappingPart(4, PartPosition.Last);

            elements.AddRange(new[] { last, two, first, one });
            elements.Sort(new MappingPartComparer());

            Assert.AreEqual(first, elements[0]);
            Assert.AreEqual(one, elements[1]);
            Assert.AreEqual(two, elements[2]);
            Assert.AreEqual(last, elements[3]);
        }

        private class StubbedMappingPart : IMappingPart
        {
            public StubbedMappingPart(int level)
                : this(level, PartPosition.Anywhere)
            {}

            public StubbedMappingPart(PartPosition position)
                : this(1, position)
            {}

            public StubbedMappingPart(int level, PartPosition position)
            {
                Level = level;
                Position = position;
            }

            public void SetAttribute(string name, string value)
            {
                throw new System.NotImplementedException();
            }

            public void SetAttributes(Attributes attrs)
            {
                throw new NotImplementedException();
            }

            public void Write(XmlElement classElement, IMappingVisitor visitor)
            {
                throw new System.NotImplementedException();
            }

            public int Level { get; private set; }
            public PartPosition Position { get; private set; }

            public override string ToString()
            {
                var value = "{ Level: " + Level + ", Position: ";

                if (Position == PartPosition.First) value += "First";
                if (Position == PartPosition.Anywhere) value += "Anywhere";
                if (Position == PartPosition.Last) value += "Last";

                value += " }";

                return value;
            }
        }
    }
}
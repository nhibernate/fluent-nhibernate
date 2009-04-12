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

            elements.AddRange(new[] { four, two, one, three });
            elements.Sort(new MappingPartComparer(elements));

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
            elements.Sort(new MappingPartComparer(elements));

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
            elements.Sort(new MappingPartComparer(elements));

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
            elements.Sort(new MappingPartComparer(elements));

            Assert.AreEqual(first, elements[0]);
            Assert.AreEqual(one, elements[1]);
            Assert.AreEqual(two, elements[2]);
            Assert.AreEqual(last, elements[3]);
        }

        [Test]
        public void Parts_with_the_same_level_and_position_should_remain_in_the_same_order()
        {
            var elements = new List<IMappingPart>();

            var first = new StubbedMappingPart(1, PartPosition.First);
            var one = new StubbedMappingPart(2, PartPosition.Anywhere);
            var two = new StubbedMappingPart(3, PartPosition.Anywhere);
            var three = new StubbedMappingPart(3, PartPosition.Anywhere);
            var four = new StubbedMappingPart(3, PartPosition.Anywhere);
            var nextToLast = new StubbedMappingPart(4, PartPosition.Last);
            var five = new StubbedMappingPart(3, PartPosition.Anywhere);
            var six = new StubbedMappingPart(3, PartPosition.Anywhere);
            var second = new StubbedMappingPart(1, PartPosition.First);
            var last = new StubbedMappingPart(4, PartPosition.Last);

             elements.AddRange(new[]  { first, one, two, three, four, nextToLast, five, six, second, last });
            elements.Sort(new MappingPartComparer(elements));

            Assert.AreEqual(first, elements[0]);
            Assert.AreEqual(second, elements[1]);
            Assert.AreEqual(one, elements[2]);
            Assert.AreEqual(two, elements[3]);
            Assert.AreEqual(three, elements[4]);
            Assert.AreEqual(four, elements[5]);
            Assert.AreEqual(five, elements[6]);
            Assert.AreEqual(six, elements[7]);
            Assert.AreEqual(nextToLast, elements[8]);
            Assert.AreEqual(last, elements[9]);
        }

        private class StubbedMappingPart : IMappingPart
        {
            public StubbedMappingPart(int level)
                : this(level, PartPosition.Anywhere)
            { }

            public StubbedMappingPart(PartPosition position)
                : this(1, position)
            { }

            public StubbedMappingPart(int level, PartPosition position)
            {
                LevelWithinPosition = level;
                PositionOnDocument = position;
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

            public int LevelWithinPosition { get; private set; }
            public PartPosition PositionOnDocument { get; private set; }

            public override string ToString()
            {
                var value = "{ Level: " + LevelWithinPosition + ", Position: ";

                if (PositionOnDocument == PartPosition.First) value += "First";
                if (PositionOnDocument == PartPosition.Anywhere) value += "Anywhere";
                if (PositionOnDocument == PartPosition.Last) value += "Last";

                value += " }";

                return value;
            }
        }
    }
}
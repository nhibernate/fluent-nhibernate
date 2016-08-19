using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping.Providers;
using NUnit.Framework;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ElementPartTests
    {
        [Test]
        public void CanSetLength()
        {
            var part = new ElementPart(typeof(MappedObject));
            part.Column("test");
            part.Length(50);

            ElementMapping elementMapping = ((IElementMappingProvider)part).GetElementMapping();
            elementMapping.Columns.First().Length.ShouldEqual(50);
        }

        [Test]
        public void CanSetFormula()
        {
            var part = new ElementPart(typeof(MappedObject));
            part.Formula("formula");

            ElementMapping elementMapping = ((IElementMappingProvider)part).GetElementMapping();
            elementMapping.Formula.ShouldEqual("formula");
        }

        [Test]
      	public void CanDeclareColumnNullable()
        {
      	    var part = new ElementPart(typeof(MappedObject));
            part.Column("nullabilityCheck");
      	    part.Nullable();

            ElementMapping elementMapping = ((IElementMappingProvider)part).GetElementMapping();
            var columnMappings = elementMapping.Columns.ToList();
            columnMappings.First().Nullable.ShouldEqual(true);
        }

        [Test]
        public void CanDeclareColumnNotNullable()
        {
            var part = new ElementPart(typeof(MappedObject));
            part.Column("nullabilityCheck");
            part.Not.Nullable();

            ElementMapping elementMapping = ((IElementMappingProvider)part).GetElementMapping();
            var columnMappings = elementMapping.Columns.ToList();
            columnMappings.First().Nullable.ShouldEqual(false);
        }
    }
}

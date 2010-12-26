using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class StoredProcedurePart : IStoredProcedureMappingProvider
    {
        private readonly CheckTypeExpression<StoredProcedurePart> check;
        private readonly string _element;
        private readonly string _innerText;
        private readonly AttributeStore<StoredProcedureMapping> attributes = new AttributeStore<StoredProcedureMapping>();


        public StoredProcedurePart(string element, string innerText)
        {
            _element = element;
            _innerText = innerText;

            check = new CheckTypeExpression<StoredProcedurePart>(this, value => attributes.Set(x => x.Check, value));
        }


        public CheckTypeExpression<StoredProcedurePart> Check
        {
            get { return check; }
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public StoredProcedureMapping GetStoredProcedureMapping()
        {
            var mapping = new StoredProcedureMapping(attributes.CloneInner());

            mapping.SPType = _element;
            mapping.Query = _innerText;

            if (!mapping.IsSpecified("Check"))
                mapping.SetDefaultValue(x => x.Check, "rowcount");

            return mapping;
        }
    }
}

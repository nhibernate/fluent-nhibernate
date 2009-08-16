using System;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class JoinInstance : JoinInspector, IJoinInstance
    {
        private readonly JoinMapping mapping;
        private bool nextBool = true;

        public JoinInstance(JoinMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public IJoinInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new IFetchInstance Fetch
        {
            get
            {
                return new FetchInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Fetch))
                        mapping.Fetch = value;
                });
            }
        }

        public new void Inverse()
        {
            if (!mapping.IsSpecified(x => x.Inverse))
            {
                mapping.Inverse = nextBool;
                nextBool = true;
            }
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }

        public new void Optional()
        {
            if (!mapping.IsSpecified(x => x.Optional))
            {
                mapping.Optional = nextBool;
                nextBool = true;
            }
        }

        public new void Schema(string schema)
        {
            if (!mapping.IsSpecified(x => x.Schema))
                mapping.Schema = schema;
        }

        public void Table(string table)
        {
            if (!mapping.IsSpecified(x => x.TableName))
                mapping.TableName = table;
        }

        public new void Catalog(string catalog)
        {
            if (!mapping.IsSpecified(x => x.Catalog))
                mapping.Catalog = catalog;
        }

        public new void Subselect(string subselect)
        {
            if (!mapping.IsSpecified(x => x.Subselect))
                mapping.Subselect = subselect;
        }
    }
}
using System.Diagnostics;
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            get { return new FetchInstance(value => mapping.Set(x => x.Fetch, Layer.Conventions, value)); }
        }

        public new void Inverse()
        {
            mapping.Set(x => x.Inverse, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }

        public new void Optional()
        {
            mapping.Set(x => x.Optional, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Schema(string schema)
        {
            mapping.Set(x => x.Schema, Layer.Conventions, schema);
        }

        public void Table(string table)
        {
            mapping.Set(x => x.TableName, Layer.Conventions, table);
        }

        public new void Catalog(string catalog)
        {
            mapping.Set(x => x.Catalog, Layer.Conventions, catalog);
        }

        public new void Subselect(string subselect)
        {
            mapping.Set(x => x.Subselect, Layer.Conventions, subselect);
        }
    }
}
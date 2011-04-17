using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Visitors
{
    public class ComponentColumnPrefixVisitor : DefaultMappingModelVisitor
    {
        private Stack<string> prefixes = new Stack<string>();

        public override void Visit(IComponentMapping mapping)
        {
            StorePrefix(mapping);
            base.Visit(mapping);
            ResetPrefix();
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            if (prefixes.Any())
                columnMapping.Set(x => x.Name, Layer.UserSupplied, GetPrefix() + columnMapping.Name);
        }

        private string GetPrefix()
        {
            return string.Join("", prefixes.Reverse().ToArray());
        }

        private void StorePrefix(IComponentMapping mapping)
        {
            if (mapping.HasColumnPrefix)
                prefixes.Push(mapping.ColumnPrefix.Replace("{property}", mapping.Member.Name));
        }

        private void ResetPrefix()
        {
            if (prefixes.Count > 0)
                prefixes.Pop();
        }
    }
}
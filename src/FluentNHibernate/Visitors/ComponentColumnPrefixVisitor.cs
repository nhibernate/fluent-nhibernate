using System;
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
            if (prefixes.Any(x => x != String.Empty))
                columnMapping.Set(x => x.Name, Layer.UserSupplied, GetPrefix() + columnMapping.Name);
        }

        private string GetPrefix()
        {
            return string.Join("", prefixes.Reverse().ToArray());
        }

        private void StorePrefix(IComponentMapping mapping)
        {
            var prefix = mapping.HasColumnPrefix 
                ? mapping.ColumnPrefix.Replace("{property}", mapping.Member.Name) 
                : String.Empty;

            prefixes.Push(prefix);
        }

        private void ResetPrefix()
        {
            if (prefixes.Count > 0)
                prefixes.Pop();
        }
    }
}
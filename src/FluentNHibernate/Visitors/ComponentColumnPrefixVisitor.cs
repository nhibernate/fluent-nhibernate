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
            if (!(mapping is ReferenceComponentMapping))
            {
                base.Visit(mapping);
                return;
            }

            StorePrefix(mapping);
            base.Visit(mapping);
            ResetPrefix();
        }

        public override void ProcessComponent(ComponentMapping mapping)
        {
            base.ProcessComponent(mapping);
        }

        public override void ProcessComponent(ReferenceComponentMapping componentMapping)
        {
            componentMapping.MergedModel.AcceptVisitor(this);
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            if (prefixes.Any())
                columnMapping.Name = GetPrefix() + columnMapping.Name;
        }

        private string GetPrefix()
        {
            return string.Join("", prefixes.Reverse().ToArray());
        }

        private void StorePrefix(IComponentMapping mapping)
        {
            var referenceMapping = (ReferenceComponentMapping)mapping;

            if (referenceMapping.HasColumnPrefix)
                prefixes.Push(referenceMapping.ColumnPrefix.Replace("{property}", mapping.Member.Name));
        }

        private void ResetPrefix()
        {
            if (prefixes.Count > 0)
                prefixes.Pop();
        }
    }
}
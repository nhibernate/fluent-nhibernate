using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate
{
    public class SeparateSubclassVisitor : DefaultMappingModelVisitor
    {
        private readonly IList<IIndeterminateSubclassMappingProvider> subclassProviders;

        public SeparateSubclassVisitor(IList<IIndeterminateSubclassMappingProvider> subclassProviders)
        {
            this.subclassProviders = subclassProviders;
        }

        public override void ProcessClass(ClassMapping mapping)
        {
            var subclasses = subclassProviders
                .Select(x => x.GetSubclassMapping(CreateSubclass(mapping)))
                .Where(x => x.Type.BaseType == mapping.Type);

            foreach (var subclass in subclasses)
                mapping.AddSubclass(subclass);

            base.ProcessClass(mapping);
        }

        public override void ProcessSubclass(SubclassMapping mapping)
        {
            var subclasses = subclassProviders
                .Select(x => x.GetSubclassMapping(new SubclassMapping()))
                .Where(x => x.Type.BaseType == mapping.Type);

            foreach (var subclass in subclasses)
                mapping.AddSubclass(subclass);

            base.ProcessSubclass(mapping);
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping mapping)
        {
            var subclasses = subclassProviders
                .Select(x => x.GetSubclassMapping(new JoinedSubclassMapping()))
                .Where(x => x.Type.BaseType == mapping.Type);

            foreach (var subclass in subclasses)
                mapping.AddSubclass(subclass);

            base.ProcessJoinedSubclass(mapping);
        }

        private ISubclassMapping CreateSubclass(ClassMapping mapping)
        {
            if (mapping.Discriminator == null)
                return new JoinedSubclassMapping();
            
            return new SubclassMapping();
        }
    }
}
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Visitors
{
    /// <summary>
    /// Visitor that performs validation against the mapping model.
    /// </summary>
    public class ValidationVisitor : DefaultMappingModelVisitor
    {
        public ValidationVisitor()
        {
            Enabled = true;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            if (!Enabled) return;
            if (classMapping.Id == null)
                throw new ValidationException(
                    string.Format("The entity '{0}' doesn't have an Id mapped.", classMapping.Type.Name),
                    "Use the Id method to map your identity property. For example: Id(x => x.Id)",
                    classMapping.Type
                );
        }

        public override void ProcessCollection(CollectionMapping mapping)
        {
            if (!Enabled) return;

            var otherSide = mapping.OtherSide as CollectionMapping;

            if (otherSide == null) return;
            if (mapping.Inverse && otherSide.Inverse)
            {
                throw new ValidationException(
                    string.Format("The relationship {0}.{1} to {2}.{3} has Inverse specified on both sides.", mapping.ContainingEntityType.Name, mapping.Name, otherSide.ContainingEntityType.Name, otherSide.Name),
                    "Remove Inverse from one side of the relationship",
                    mapping.ContainingEntityType);
            }
        }

        /// <summary>
        /// Gets or sets whether validation is performed.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
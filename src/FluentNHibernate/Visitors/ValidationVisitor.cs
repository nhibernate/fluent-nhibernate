using FluentNHibernate.MappingModel.ClassBased;

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

        /// <summary>
        /// Gets or sets whether validation is performed.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
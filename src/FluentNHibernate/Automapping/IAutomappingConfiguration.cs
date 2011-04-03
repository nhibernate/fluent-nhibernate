using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    /// <summary>
    /// Implement this interface to control how the automapper behaves.
    /// Typically you're better off deriving from the <see cref="DefaultAutomappingConfiguration"/>
    /// class, which is pre-configured with the default settings; you can then
    /// just override specific methods that you'd like to alter.
    /// </summary>
    public interface IAutomappingConfiguration
    {
        /// <summary>
        /// Determines whether a type should be auto-mapped.
        /// Override to restrict which types are mapped in your domain.
        /// </summary>
        /// <remarks>
        /// You normally want to override this method and restrict via something known, like
        /// Namespace.
        /// </remarks>
        /// <example>
        /// return type.Namespace.EndsWith("Domain");
        /// </example>
        /// <param name="type">Type to map</param>
        /// <returns>Should map type</returns>
        bool ShouldMap(Type type);

        /// <summary>
        /// Determines whether a member of a type should be auto-mapped.
        /// Override to restrict which members are considered in automapping.
        /// </summary>
        /// <remarks>
        /// You normally want to override this method to restrict which members will be
        /// used for mapping. This method will be called for every property, field, and method
        /// on your types.
        /// </remarks>
        /// <example>
        /// // all writable public properties:
        /// return member.IsProperty &amp;&amp; member.IsPublic &amp;&amp; member.CanWrite;
        /// </example>
        /// <param name="member">Member to map</param>
        /// <returns>Should map member</returns>
        bool ShouldMap(Member member);

        /// <summary>
        /// Determines whether a member is the id of an entity.
        /// </summary>
        /// <remarks>
        /// This method is called for each member that ShouldMap(Type) returns true for.
        /// </remarks>
        /// <param name="member">Member</param>
        /// <returns>Member is id</returns>
        bool IsId(Member member);

        /// <summary>
        /// Gets the access strategy to be used for a read-only property. This method is
        /// called for every setterless property and private-setter autoproperty in your
        /// domain that has been accepted through <see cref="ShouldMap(FluentNHibernate.Member)"/>.
        /// </summary>
        /// <param name="member">Member to get access strategy for</param>
        /// <returns>Access strategy</returns>
        Access GetAccessStrategyForReadOnlyProperty(Member member);

        /// <summary>
        /// Controls which side of a many-to-many relationship is considered the "parent".
        /// </summary>
        /// <param name="left">Left side of the relationship</param>
        /// <param name="right">Right side of the relationship</param>
        /// <returns>left or right</returns>
        Type GetParentSideForManyToMany(Type left, Type right);

        /// <summary>
        /// Determines whether a type is a concrete, or instantiatable, base class. This
        /// affects how the inheritance mappings are built, specifically that any types
        /// this method returns true for will not be mapped as a subclass.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Base type is concrete?</returns>
        bool IsConcreteBaseType(Type type);

        /// <summary>
        /// Specifies that a particular type should be mapped as a component rather than
        /// an entity.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Type is a component?</returns>
        bool IsComponent(Type type);

        /// <summary>
        /// Gets the column prefix for a component.
        /// </summary>
        /// <param name="member">Member defining the component</param>
        /// <returns>Component column prefix</returns>
        string GetComponentColumnPrefix(Member member);

        /// <summary>
        /// Specifies whether a particular type is mapped with a discriminator.
        /// This method will be called for every type that has already been
        /// approved by <see cref="ShouldMap(System.Type)"/>.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>Whether the type is to be discriminated</returns>
        bool IsDiscriminated(Type type);

        /// <summary>
        /// Gets the column name of the discriminator.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Discriminator column name</returns>
        string GetDiscriminatorColumn(Type type);

        [Obsolete("Use IsDiscriminated instead.", true)]
        SubclassStrategy GetSubclassStrategy(Type type);

        /// <summary>
        /// Specifies whether an abstract type is considered a Layer Supertype
        /// (http://martinfowler.com/eaaCatalog/layerSupertype.html). Defaults to
        /// true for all abstract classes. Override this method if you have an
        /// abstract class that you want mapping as a regular entity.
        /// </summary>
        /// <param name="type">Abstract class type</param>
        /// <returns>Whether the type is a Layer Supertype</returns>
        bool AbstractClassIsLayerSupertype(Type type);

        /// <summary>
        /// Gets the value column for a collection of simple types.
        /// </summary>
        /// <remarks>
        /// This is the name of the &lt;element&gt; column.
        /// </remarks>
        /// <param name="member">Collection property</param>
        /// <returns>Value column name</returns>
        string SimpleTypeCollectionValueColumn(Member member);

        /// <summary>
        /// Specifies whether the current member is a version property
        /// </summary>
        /// <param name="member">Candidate member</param>
        /// <returns>Is member a version</returns>
        bool IsVersion(Member member);

        /// <summary>
        /// Gets the steps that are executed to map a type.
        /// </summary>
        /// <returns>Collection of mapping steps</returns>
        // TODO: Remove need for ConventionFinder and AutoMapper references here
        IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping
{
    /// <summary>
    /// Implement this interface to control how the automapper behaves.
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
        /// return member.IsProperty && member.IsPublic && member.CanWrite;
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

        Type GetParentSideForManyToMany(Type left, Type right);
        bool IsConcreteBaseType(Type type);
        bool IsComponent(Type type);
        string GetComponentColumnPrefix(Member member);
        bool IsDiscriminated(Type type);
        string GetDiscriminatorColumn(Type type);
        SubclassStrategy GetSubclassStrategy(Type type);
        bool AbstractClassIsLayerSupertype(Type type);
        string SimpleTypeCollectionValueColumn(Member member);

        /// <summary>
        /// Gets the steps that are executed to map a type.
        /// </summary>
        /// <returns>Collection of mapping steps</returns>
        // TODO: Remove need for ConventionFinder and AutoMapper references here
        IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder);
    }
}
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using NHibernate;

namespace FluentNHibernate.Testing
{
    public class PersistenceSpecification<T>
    {
    	protected readonly List<Property<T>> allProperties = new List<Property<T>>();
        private readonly ISession currentSession;
        private readonly IEqualityComparer entityEqualityComparer;
        private readonly bool hasExistingSession;

        public PersistenceSpecification(ISessionSource source)
            : this(source.CreateSession())
        {
        }

        public PersistenceSpecification(ISessionSource source, IEqualityComparer entityEqualityComparer)
            : this(source.CreateSession(), entityEqualityComparer)
        {
        }

        public PersistenceSpecification(ISession session)
            : this(session, null)
        {
        }

        public PersistenceSpecification(ISession session, IEqualityComparer entityEqualityComparer)
        {
            currentSession = session;
            hasExistingSession = currentSession.Transaction != null && currentSession.Transaction.IsActive;
            this.entityEqualityComparer = entityEqualityComparer;
        }

    	public void VerifyTheMappings()
        {
            // CreateProperties the initial copy
            var first = typeof(T).InstantiateUsingParameterlessConstructor<T>();

            // Set the "suggested" properties, including references
            // to other entities and possibly collections
            allProperties.ForEach(p => p.SetValue(first));

            // Save the first copy
            TransactionalSave(first);

            object firstId = currentSession.GetIdentifier(first);

            // Clear and reset the current session
            currentSession.Flush();
            currentSession.Clear();

            // "Find" the same entity from the second IRepository
            var second = currentSession.Get<T>(firstId);

            // Validate that each specified property and value
            // made the round trip
            // It's a bit naive right now because it fails on the first failure
            allProperties.ForEach(p => p.CheckValue(second));
        }

    	public void TransactionalSave(object propertyValue)
        {
            if (hasExistingSession)
            {
                currentSession.Save(propertyValue);
            }
            else
            {
                using (var tx = currentSession.BeginTransaction())
                {
                    currentSession.Save(propertyValue);
                    tx.Commit();
                }
            }
        }

        public PersistenceSpecification<T> RegisterCheckedProperty(Property<T> property)
        {
            property.EntityEqualityComparer = entityEqualityComparer;
            allProperties.Add(property);

            property.HasRegistered(this);

            return this;
        }
    }
}

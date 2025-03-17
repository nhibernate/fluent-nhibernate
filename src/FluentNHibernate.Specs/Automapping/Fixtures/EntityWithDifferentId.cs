using System;

namespace FluentNHibernate.Specs.Automapping.Fixtures;

public class EntityWithDifferentId
{
    public virtual int DestinationId { get; set; }
    public virtual Guid Id { get; set; }
}

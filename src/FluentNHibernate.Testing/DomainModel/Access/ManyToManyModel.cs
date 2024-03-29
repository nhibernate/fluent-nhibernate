using System.Collections.Generic;

namespace FluentNHibernate.Testing.DomainModel.Access;

class ManyToManyModel
{
    public virtual int Id { get; private set; }
    public IList<ParentModel> Bag { get; private set; }
}

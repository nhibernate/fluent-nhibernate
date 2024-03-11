﻿namespace FluentNHibernate.Testing.DomainModel.Access;

class CompositeIdModel
{
    public virtual int IdA { get; private set; }
    public virtual int IdB { get; private set; }


    public override int GetHashCode()
    {
        return (IdA * 23) ^ IdB;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as CompositeIdModel);
    }

    public virtual bool Equals(CompositeIdModel other)
    {
        return other is not null && other.IdA == this.IdA && other.IdB == this.IdB;
    }
}

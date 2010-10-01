using System;

namespace FluentNHibernate.Diagnostics
{
    public class SkippedAutomappingType
    {
        public Type Type { get; set; }
        public string Reason { get; set; }

        public bool Equals(SkippedAutomappingType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Type, Type) && Equals(other.Reason, Reason);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SkippedAutomappingType)) return false;
            return Equals((SkippedAutomappingType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Reason != null ? Reason.GetHashCode() : 0);
            }
        }
    }
}
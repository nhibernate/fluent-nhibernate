using System;
using System.Runtime.Serialization;

namespace FluentNHibernate
{
    [Serializable]
    public class UnknownPropertyException : Exception
    {
        public UnknownPropertyException(Type classType, string propertyName)
        {
            Type = classType;
            Property = propertyName;
        }

        public string Property { get; private set; }
        public Type Type { get; private set; }

        protected UnknownPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Type = info.GetValue("Type", typeof(Type)) as Type;
            this.Property = info.GetString("Property");
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Type", Type, typeof(Type));
            info.AddValue("Property", Property);
        }
    }
}
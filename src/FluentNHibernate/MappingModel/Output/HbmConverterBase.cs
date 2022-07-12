using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class HbmConverterBase<F, H> : NullMappingModelVisitor, IHbmConverter<F, H>
    {
        private readonly IHbmConverterServiceLocator serviceLocator;

        protected HbmConverterBase(IHbmConverterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public abstract H Convert(F mapping);

        protected HSub ConvertFluentSubobjectToHibernateNative<FSub, HSub>(FSub fluentMapping)
        {
            var converter = serviceLocator.GetConverter<FSub, HSub>();
            return converter.Convert(fluentMapping);
        }

        protected void AddToNullableArray<T>(ref T[] nullableArray, T item)
        {
            if (nullableArray == null)
                nullableArray = new T[0];
            Array.Resize(ref nullableArray, nullableArray.Length + 1);
            nullableArray[nullableArray.Length - 1] = item;
        }

        protected E LookupEnumValueIn<E>(XmlLinkedEnumBiDictionary<E> enumDict, String key)
                where E : System.Enum
        {
            try
            {
                return enumDict[key];
            }
            catch (KeyNotFoundException keyEx)
            {
                throw new NotSupportedException(String.Format("{0} is not a recognized value for {1}", key, typeof(E).Name), keyEx);
            }
        }
    }
}
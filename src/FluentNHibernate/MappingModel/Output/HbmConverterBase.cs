using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class HbmConverterBase<F, H> : NullMappingModelVisitor, IHbmConverter<F, H>
        where F: IMapping
    {
        private readonly IHbmConverterServiceLocator serviceLocator;

        protected HbmConverterBase(IHbmConverterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public abstract H Convert(F mapping);

        protected HSub ConvertFluentSubobjectToHibernateNative<FSub, HSub>(FSub fluentMapping)
            where FSub : IMapping
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
    }
}
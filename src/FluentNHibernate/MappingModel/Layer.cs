using System;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public static class Layer
    {
        public const int Defaults = 0;
        public const int Conventions = 1;
        public const int UserSupplied = 2;
    }
}
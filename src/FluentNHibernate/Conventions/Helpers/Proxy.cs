using FluentNHibernate.Conventions.Helpers.Prebuilt;
namespace FluentNHibernate.Conventions.Helpers
{
    public class Proxy<TProxyType>
    {
        public static ProxyConvention UsedForType<TPersistentType>()
        {
            return new BuiltProxyConvention(typeof(TProxyType), typeof(TPersistentType));
        }
    }
}
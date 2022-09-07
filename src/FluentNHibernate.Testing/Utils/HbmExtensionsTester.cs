using System.Collections.Generic;
using NHibernate;
using NHibernate.Type;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmExtensionsTester
    {
        [Test]
        public void ShouldConvertHbmParam()
        {
            var paramPair = new KeyValuePair<string, string>("george", "fred");

            var convertedParameter = FluentNHibernate.Utils.HbmExtensions.ToHbmParam(paramPair);

            convertedParameter.ShouldNotBeNull();
            convertedParameter.name.ShouldEqual(paramPair.Key);
            convertedParameter.Text.ShouldEqual(new string[] { paramPair.Value });
        }

        [Test]
        public void ShouldConvertHbmFilterParam()
        {
            var paramPair = new KeyValuePair<string, IType>("george", NHibernateUtil.Int32);

            var convertedParameter = FluentNHibernate.Utils.HbmExtensions.ToHbmFilterParam(paramPair);

            convertedParameter.ShouldNotBeNull();
            convertedParameter.name.ShouldEqual(paramPair.Key);
            convertedParameter.type.ShouldEqual(paramPair.Value.Name);
        }
    }
}

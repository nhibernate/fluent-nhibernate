using System;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Utils
{
    public static class Mock<T> where T : class
    {
        public static T Create()
        {
            return Create(x => { });
        }

        public static T Create(Action<T> setup)
        {
            var mock = MockRepository.GenerateMock<T>();

            setup(mock);

            return mock;
        }
    }
}

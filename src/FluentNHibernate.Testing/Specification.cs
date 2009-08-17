using System;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace FluentNHibernate.Testing
{
    [TestFixture]
    public class Specification
    {
        public Exception thrown_exception { get; private set; }

        [SetUp]
        public void Setup()
        {
            establish_context();
            try
            {
                because();
            }
            catch (Exception e)
            {
                thrown_exception = e;
            }
        }

        public virtual void establish_context()
        {
        }

        /// <summary>
        /// An optional method you can use after <see cref="establish_context"/> to exercise the
        /// system under test. Also, any exception raised during the <see cref="because"/> will be 
        /// captured and available for inspection via the <see cref="thrown_exception"/> property.
        /// </summary>
        public virtual void because()
        {
        }

        [TearDown]
        public void Teardown()
        {
            after_each();
        }

        public virtual void after_each()
        {
        }

        public T test_double<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        public T test_double<T>(params object[] args) where T : class
        {
            return MockRepository.GenerateStub<T>(args);
        }

        protected void raise_error(object mock, string eventName, object sender, EventArgs args)
        {
            new EventRaiser((IMockedObject) mock, eventName).Raise(sender, args);
        }

        protected void spec_not_implemented()
        {
            MethodBase caller = new StackTrace().GetFrame(1).GetMethod();

            spec_not_implemented(caller.DeclaringType.Name + "." + caller.Name);
        }

        protected void spec_not_implemented(string specName)
        {
            Console.WriteLine("Specification not implemented : " + specName);
        }
    }
}

using System;
using fit;
using fitlibrary;
using FluentNHibernate.Framework;
using FluentNHibernate.Framework.Fixtures;
using ShadeTree.Validation;

namespace FluentNHibernate.Framework.Fixtures
{
    public class GenericRowFixture<T> : RowFixture where T : class
    {
        private T[] _array;

        public GenericRowFixture(T[] array)
        {
            _array = array;
        }

        public override object[] Query()
        {
            return _array;
        }

        public override Type GetTargetClass()
        {
            return typeof(T);
        }
    }

    public class DomainClassFixture<T> : DoFixture where T : Entity, new()
    {
        private T _subject;
        private Action<T> _onFinished = t => { };
        private readonly IRepository repository;

        public DomainClassFixture(IRepository repository)
        {
            _subject = new T();
            this.repository = repository;
        }

        public DomainClassFixture(T subject, IRepository repository)
        {
            _subject = subject;
            this.repository = repository;
        }

        [Example("|Find|[id]|")]
        public void Find(long id)
        {
            _subject = repository.Find<T>(id);

            if (_subject == null)
            {
                string message = string.Format("Could not find {0}.Id = {1}", typeof (T).Name, id);
                throw new ApplicationException(message);
            }
        }

        [Example("|Find by alias|[alias]|")]
        public void FindByAlias(string alias)
        {
            long id = TestContext.GetValue<long>(alias);
            Find(id);
        }

        [Example("|Save as alias|[alias]|")]
        public void SaveAsAlias(string alias)
        {
            repository.Save(_subject);

            TestContext.StoreAlias(alias, _subject.Id);
        }


        public Action<T> OnFinished
        {
            get { return _onFinished; }
            set { _onFinished = value; }
        }

        public override void DoRows(Parse theRows)
        {
            base.DoRows(theRows);
            _onFinished(Subject);
        }

        public T Subject
        {
            get { return _subject; }
        }


        [Example("|There are no validation messages for|[fieldName]|")]
        public bool ThereAreNoValidationMessagesFor(string fieldName)
        {
            return Validator.ValidateField(Subject, fieldName).Length == 0;
        }

        [Example(@"|The Validation Messages For|[fieldName]|are|
|Message|
|[message]|")]
        public Fixture TheValidationMessagesForAre(string fieldName)
        {
            NotificationMessage[] messages = Validator.ValidateField(Subject, fieldName);
            return new GenericRowFixture<NotificationMessage>(messages);
        }

        [Example("|The Validation Messages For|[fieldName]|contains|[message]|")]
        public bool TheValidationMessagesForContains(string fieldName, string message)
        {
            NotificationMessage[] messages = Validator.ValidateField(Subject, fieldName);
            bool correct = Array.Find(messages, m => m.Message == message) != null;

            if (!correct)
            {
                throwErrorMessagesAreWrong(messages);
            }

            return true;
        }

        private void throwErrorMessagesAreWrong(NotificationMessage[] messages)
        {
            string errorMessage = "\n\nWrong messages!";
            foreach (NotificationMessage notificationMessage in messages)
            {
                errorMessage += "\n" + notificationMessage.ToString();
            }

            errorMessage += "\n\n\n";

            throw new ApplicationException(errorMessage);
        }

        [Example("|The Validation Messages For|[fieldName]|does not contain|[message]|")]
        public bool TheValidationMessagesForDoesNotContain(string fieldName, string message)
        {
            NotificationMessage[] messages = Validator.ValidateField(Subject, fieldName);
            bool correct = Array.Find(messages, m => m.Message == message) == null;

            if (!correct)
            {
                throwErrorMessagesAreWrong(messages);
            }

            return true;
        }
    }
}
using System.Collections.Generic;
using NHibernate.Collection.Generic;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class Blog : EntityBase
    {
        public IDictionary<string, string> UrlAliases { get; set; }
        public IDictionary<string, string> UrlAliasesCustomCollection { get; set; }
        public IDictionary<string, Post> Permalinks { get; set; }
        public IDictionary<Post, User> Commentors { get; set; }
        public CommentorCollection CommentorCustomCollection { get; set; }
    }

    class User : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Post : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class CommentorCollection : PersistentGenericMap<Post, User>
    {}

    class CustomMap<TKey, TValue> : PersistentGenericMap<TKey, TValue>
    { }
}
using Iesi.Collections.Generic;

namespace FluentNHibernate.Testing.Automapping
{
    namespace ManyToMany
    {
        public class ManyToMany1
        {
            public virtual int Id { get; set; }
            public virtual ISet<ManyToMany2> Many1 { get; set; }
        }

        public class ManyToMany2
        {
            public virtual int Id { get; set; }
            public virtual ISet<ManyToMany1> Many2 { get; set; }
        }
    }
}

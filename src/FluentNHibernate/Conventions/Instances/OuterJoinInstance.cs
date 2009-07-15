using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class OuterJoinInstance : IOuterJoinInstance
    {
        private readonly Action<string> setter;

        public OuterJoinInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Auto()
        {
            setter("auto");
        }

        public void Yes()
        {
            setter("true");
        }

        public void No()
        {
            setter("false");
        }
    }
}
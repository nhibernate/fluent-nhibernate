namespace FluentNHibernate.Conventions.Instances
{
    public interface IReadOnlyInstance
    {
        /// <summary> Shortcut for setting <c>.Not.Insert().Not.Update()</c>. </summary>
        void ReadOnly();
    }
}
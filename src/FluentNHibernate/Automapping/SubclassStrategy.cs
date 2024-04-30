#if USE_NULLABLE
#nullable enable
#endif
using System;

namespace FluentNHibernate.Automapping;

[Obsolete("Use IsDiscriminated in IAutomappingConfiguration instead")]
public enum SubclassStrategy
{
    JoinedSubclass,
    Subclass
}

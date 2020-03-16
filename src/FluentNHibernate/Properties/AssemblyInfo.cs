using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("FluentHibernate")]
[assembly: AssemblyDescription("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
#if !STRONG_NAME
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("FluentNHibernate.Testing")]
#endif
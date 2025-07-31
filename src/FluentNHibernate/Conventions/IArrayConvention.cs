﻿using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

[Obsolete("Use ICollectionConvention with an acceptance criteria")]
public interface IArrayConvention : IConvention<IArrayInspector, IArrayInstance>
{
}

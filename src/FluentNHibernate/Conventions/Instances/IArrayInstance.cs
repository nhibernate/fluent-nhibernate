﻿using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances;

[Obsolete("Use IConventionInstance")]
public interface IArrayInstance : IArrayInspector, ICollectionInstance
{}

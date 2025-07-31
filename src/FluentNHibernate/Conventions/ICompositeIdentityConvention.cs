﻿using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

public interface ICompositeIdentityConvention : IConvention<ICompositeIdentityInspector, ICompositeIdentityInstance>
{}

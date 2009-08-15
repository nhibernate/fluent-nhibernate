using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            autoMapper = new PrivateAutoMapper(Expressions, new DefaultConventionFinder(), inlineOverrides);
        }
    }
}
using System.Linq;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmGeneratorConverter : HbmConverterBase<GeneratorMapping, HbmGenerator>
    {
        private HbmGenerator hbmGenerator;

        public HbmGeneratorConverter() : base(null)
        {
        }

        public override HbmGenerator Convert(GeneratorMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmGenerator;
        }

        public override void ProcessGenerator(GeneratorMapping generatorMapping)
        {
            hbmGenerator = new HbmGenerator();

            if (!string.IsNullOrEmpty(generatorMapping.Class))
                hbmGenerator.@class = generatorMapping.Class;

            if (generatorMapping.Params.Any())
                hbmGenerator.param = generatorMapping.Params.Select(paramPair => paramPair.ToHbmParam()).ToArray();
        }
    }
}

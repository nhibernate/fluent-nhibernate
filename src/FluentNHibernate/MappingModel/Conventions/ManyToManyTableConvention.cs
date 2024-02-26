//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FluentNHibernate.MappingModel.Collections;

//namespace FluentNHibernate.MappingModel.Conventions
//{
//    public class ManyToManyTableConvention : DefaultMappingModelVisitor
//    {
//        private IList<ManyToManyMapping> _foundMappings = new List<ManyToManyMapping>();

//        public Func<ManyToManyMapping, ManyToManyMapping, string> DetermineTableName =
//            (sideA, sideB) => string.Format("{0}To{1}", sideA, sideB);

//        public override ProcessManyToMany(ManyToManyMapping manyToManyMapping)
//        {
//            ManyToManyMapping otherSide = _foundMappings.Where(
//                m => m.ParentType == manyToManyMapping.ChildType && m.ChildType == manyToManyMapping.ParentType)
//                .FirstOrDefault();
                
//            if(otherSide == null)
//                _foundMappings.Add(manyToManyMapping);
//            else
//            {
//                string tableName = DetermineTableName(manyToManyMapping, otherSide);
//                manyToManyMapping.Table = tableName;
//                otherSide.Table = tableName;
//                _foundMappings.Remove(otherSide);
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FluentNHibernate.Xml
{
    public class MappingXmlValidator
    {
        public SchemaValidationResult Validate(XmlDocument document)
        {
            if (document.Schemas.Count == 0)
                throw new InvalidOperationException("The document must have a schema assigned.");

            var result = new SchemaValidationResult();

            document.Validate((sender, e) => 
                {
                    // This stinks, there has got to be a better way.
                    string source = null;
                    var ex = e.Exception as XmlSchemaValidationException;
                    if(ex != null)
                    {
                        var element = ex.SourceObject as XmlElement;
                        if (element != null)
                            source = string.Format("Element '{0}'", element.Name);
                    }                    

                    result.Messages.Add(string.Format("{0}: {1}", source ?? "no source", e.Message));
                });

            return result;
        }

        public class SchemaValidationResult
        {            
            public List<string> Messages { get; set; }

            public SchemaValidationResult()
            {
                Messages = new List<string>();
            }

            public bool Success
            {
                get { return Messages.Count == 0; }
            }

            public string FullMessageLog
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string message in Messages)
                        builder.AppendLine(message);

                    return builder.ToString();
                }
            }
        }
    }
}
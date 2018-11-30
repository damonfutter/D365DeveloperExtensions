using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionPackager.PostUnpack
{
    public class ObjectTypeCodeFixer : IDocFixer
    {
        private int objectTypeCode = 12001;

        public void Fix(XDocument doc)
        {
            fixObjectTypeCodeInElement(doc, "ObjectTypeCode"); // Entity.xml
            fixObjectTypeCodeInElement(doc, "returnedtypecode"); // Entity.xml
            fixObjectTypeCodeInElement(doc, "PrimaryEntityTypeCode"); // Entity.xml (in CustomControlDefaultConfig)

            fixObjectTypeCodeInAttribute(doc, "grid", "object"); // SavedQueries/guid-nnnnn-nnnnnn.xml

            if (doc.Element("Entity") != null)
            {
                // This was a new entity, so increment the object type code ready for the next entity.
                objectTypeCode++;
            }
        }

        private void fixObjectTypeCodeInElement(XDocument doc, string elementName)
        {
            var xml = doc.Descendants(elementName);
            xml.ToList().ForEach(x =>
            {
                int temp;
                if (int.TryParse(x.Value, out temp)
                    && temp != objectTypeCode)
                {
                    x.Value = objectTypeCode.ToString();
                }
            });
        }

        private void fixObjectTypeCodeInAttribute(XDocument doc, string elementName, string attributeName)
        {
            var xml = doc.Descendants(elementName);
            xml.ToList().ForEach(x =>
            {
                var attr = x.Attribute(attributeName);
                int temp;
                if (int.TryParse(attr.Value, out temp)
                    && temp != objectTypeCode)
                {
                    attr.Value = objectTypeCode.ToString();
                }
            });
        }
    }
}

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
        private int objectTypeCode = 10100;

        public void Fix(XDocument doc)
        {
            if (doc.Element("Entity") != null 
                && doc.Element("Entity").Element("ObjectTypeCode") != null)
            {
                int otc;
                if (int.TryParse(doc.Element("Entity").Element("ObjectTypeCode").Value, out otc)
                    && isCustomEntity(otc))
                {
                    // This is a new custom entity, so increment the object type code
                    objectTypeCode++;
                }
            }

            fixObjectTypeCodeInElement(doc, "ObjectTypeCode"); // Entity.xml
            fixObjectTypeCodeInElement(doc, "returnedtypecode"); // Entity.xml
            fixObjectTypeCodeInElement(doc, "PrimaryEntityTypeCode"); // Entity.xml (in CustomControlDefaultConfig)

            fixObjectTypeCodeInAttribute(doc, "grid", "object"); // SavedQueries/guid-nnnnn-nnnnnn.xml

        }

        private void fixObjectTypeCodeInElement(XDocument doc, string elementName)
        {
            var xml = doc.Descendants(elementName);
            xml.ToList().ForEach(x =>
            {
                int temp;
                if (int.TryParse(x.Value, out temp)
                    && temp != objectTypeCode
                    && isCustomEntity(temp))
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
                    && temp != objectTypeCode
                    && isCustomEntity(temp))
                {
                    attr.Value = objectTypeCode.ToString();
                }
            });
        }

        private bool isCustomEntity(int objectTypeCode)
        {
            return objectTypeCode >= 10000;
        }
    }
}

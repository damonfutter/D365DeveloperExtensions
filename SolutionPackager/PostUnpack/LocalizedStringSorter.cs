using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionPackager.PostUnpack
{
    public class LocalizedStringSorter : IDocFixer
    {
        public void Fix(XDocument doc)
        {
            sortContainerByAttributeValue(doc, "labels", "languagecode");
            sortContainerByAttributeValue(doc, "displaynames", "languagecode");
            sortContainerByAttributeValue(doc, "Descriptions", "languagecode"); // Entity.xml and others
            sortContainerByAttributeValue(doc, "LocalizedNames", "languagecode"); // Entity.xml, Solution.xml
            sortContainerByAttributeValue(doc, "LocalizedCollectionNames", "languagecode"); // Entity.xml
            sortContainerByAttributeValue(doc, "Descriptions", "LCID"); // Sitemap
            sortContainerByAttributeValue(doc, "Titles", "LCID"); // Sitemap
            sortContainerByAttributeValue(doc, "CustomLabels", "languagecode"); // Relationships
            sortContainerByAttributeValue(doc, "AppModuleRoleMaps", "id"); // AppModule.xml
        }

        private static void sortContainerByAttributeValue(XDocument doc, string containerElementName, string nameOfAttributeToSortBy)
        {
            var xml = doc.Descendants(containerElementName);

            xml.ToList().ForEach(x =>
            {
                // Only sort elements that have the required attribute
                var elementsToSort = x.Elements().Where(el => el.Attribute(nameOfAttributeToSortBy) != null);
                var sorted = elementsToSort.OrderBy(s => s.Attribute(nameOfAttributeToSortBy).Value).ToList();
                elementsToSort.Remove();
                sorted.ForEach(e =>
                {
                    x.Add(e);
                });
            });
        }
    }
}

using D365DeveloperExtensions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionPackager.PostUnpack
{
    public class PluginTypeIdMapper : IDocFixer
    {
        private Dictionary<string, string> pluginsToMap; // assemblyName, referenceGuid(from config file) ?
        private Dictionary<string, string> discoveredPlugins = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // guid, referenceGuid ?

        public PluginTypeIdMapper(List<PluginTypeIdMap> maps)
        {
            if (maps != null)
            {
                pluginsToMap = maps.ToDictionary(m => m.assemblyQualifiedName, m => m.pluginTypeId);
            }
        }

        public void Fix(XDocument doc)
        {
            string referenceGuid;
            if (pluginsToMap != null)
            {
                // If it's the PluginAssembly document, pull out all the PlugnTypeIds so we can use them later.
                if (doc.Element("PluginAssembly") != null
                    && doc.Element("PluginAssembly").Element("PluginTypes") != null)
                {
                    var xml = doc.Element("PluginAssembly").Element("PluginTypes").Descendants("PluginType");
                    xml.ToList().ForEach(x =>
                    {
                        if (pluginsToMap.TryGetValue(x.Attribute("AssemblyQualifiedName").Value, out referenceGuid))
                        {
                            discoveredPlugins[x.Attribute("PluginTypeId").Value] = referenceGuid;
                            x.Attribute("PluginTypeId").Value = referenceGuid;
                        }
                    });
                }

                // If it's a SdkMessageProcessingStep document, re-map its guid if necessary
                if (doc.Element("SdkMessageProcessingStep") != null
                    && doc.Element("SdkMessageProcessingStep").Element("PluginTypeId") != null)
                {
                    if (discoveredPlugins.TryGetValue(doc.Element("SdkMessageProcessingStep").Element("PluginTypeId").Value, out referenceGuid))
                    {
                        doc.Element("SdkMessageProcessingStep").Element("PluginTypeId").Value = referenceGuid;
                    }
                }
            }
        }
    }
}

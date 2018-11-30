using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionPackager.PostUnpack
{
    public class FileEnumerator
    {
        private IEnumerable<IDocFixer> fixers;
        public FileEnumerator(IEnumerable<IDocFixer> fixers)
        {
            this.fixers = fixers;
        }

        public void FixAllFiles(string directory)
        {
            bool hasChanged;
            var files = Directory.EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories);
            files.ToList().ForEach(filename =>
                {
                    var doc = XDocument.Load(filename);
                    if (doc.Root.Name == "Workflow")
                    {
                        Console.WriteLine("Skipped workflow");
                    }
                    else
                    {
                        var unsortedDoc = new XDocument(doc);
                        foreach (var fixer in this.fixers)
                        {
                            fixer.Fix(doc);
                        }

                        hasChanged = !XNode.DeepEquals(unsortedDoc, doc);
                        if (hasChanged)
                        {
                            doc.Save(filename);
                        }
                    }
                });
        }
    }
}

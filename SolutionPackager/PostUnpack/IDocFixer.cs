using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolutionPackager.PostUnpack
{
    public interface IDocFixer
    {
        void Fix(XDocument doc);
    }
}

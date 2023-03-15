using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLParser.Model
{
    public class Attribute : IXmlElement
    {
        public Node Node { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Value { get; set; }

        public Attribute(Node node, string shortName, string fullName, string value)
        {
            Node = node;
            ShortName = shortName;
            FullName = fullName;
            Value = value;
        }
    }
}

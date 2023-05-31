using System.Text;

namespace ParserXml.Model
{
    public class Node : IXmlElement
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string? Value { get; set; }
        public List<Node> ChildNodes { get; set; }
        public List<Attribute> Attributes { get; set; }


        public string AttributesStringRepresentation
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Attributes.Count; i++)
                {
                    sb.Append(Attributes[i].FullName);
                    sb.Append(" : ");
                    sb.Append(Attributes[i].Value);

                    if (i != Attributes.Count - 1)
                        sb.Append("\n");
                }

                return sb.ToString();
            }
        }


        public Node()
        {
            ChildNodes = new List<Node>();
            Attributes = new List<Attribute>();
        }

        public Node(string shortName, string fullName, string? value)
            : this()
        {
            ShortName = shortName;
            FullName = fullName;
            Value = value;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}

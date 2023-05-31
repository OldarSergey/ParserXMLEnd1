using ParserXml.Model;
using System.Xml;

namespace ParserXml.Service
{
    public interface IXmlFileParser
    {
        public XmlDocument XmlDocument { get; set; }
        public List<Node> Nodes { get; set; }
        public string FilePath { get; set; }
        public Node MapXmlNode(XmlNode xmlNode);
        public Task <bool> UploadFile(IFormFile file);
        public string GetFileSizeInKilobytes(IFormFile file);
        public Task<Node> FindNodeAsync(IEnumerable<Node> nodes, string buttonText); 

    }
}

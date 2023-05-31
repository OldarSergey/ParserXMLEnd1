using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using ParserXml.Service;
using ParserXml.Model;

namespace XMLParser.Service
{
    public class XmlFileParser : IXmlFileParser
    {
        private XmlDocument _xmlDocument;
        private List<ElementStringRepresentation> _elementStringRepresentations;
        private List<Node> _nodes;
        public XmlDocument XmlDocument

        {
            get => _xmlDocument;

            set //add check
            {
                _xmlDocument = value;
                _nodes.Clear();
                _xmlDocument.Load(FilePath);
                Nodes.Add(GetRootNode(_xmlDocument));
            }
        }
        public List<ElementStringRepresentation> ElementStringRepresentations
        { get => _elementStringRepresentations; set => _elementStringRepresentations = value; }
        public List<Node> Nodes { get => _nodes; set => _nodes = value; }
        public string FilePath { get; set; }



        //по дефолту переводит регистр
        public XmlFileParser()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LoadElementStringRepresentationsFromJson();
            Nodes = new List<Node>();
        }


        //получение корневого узла
        public Node GetRootNode(XmlDocument xmlDocument)
        {
            var rootXmlNode = xmlDocument.DocumentElement;
            var rootNode = MapXmlNode(rootXmlNode);

            return rootNode;
        }

        //подгрузка json файла
        public void LoadElementStringRepresentationsFromJson()
        {
            using (StreamReader r = new StreamReader("invoice.json"))
            {
                var json = r.ReadToEnd();
                _elementStringRepresentations = JsonConvert.DeserializeObject<List<ElementStringRepresentation>>(json);
            }
        }

        //парс
        public Node MapXmlNode(XmlNode xmlNode)
        {
            var shortName = xmlNode.Name == "#text" ? xmlNode.Value : xmlNode.Name;

            var node = new Node(shortName, GetFullName(shortName), xmlNode.Value);

            var attributeCount = xmlNode.Attributes?.Count ?? 0;

            for (int i = 0; i < attributeCount; i++)
            {
                var item = xmlNode.Attributes[i];
                var a = new ParserXml.Model.Attribute(node, item.Value, GetFullName(item.Name), item.Value);
                node.Attributes.Add(a);
            }
            var childCount = xmlNode.ChildNodes?.Count ?? 0;

            for (int i = 0; i < childCount; i++)
            {
                var item = xmlNode.ChildNodes[i];
                var n = MapXmlNode(item);
                node.ChildNodes.Add(n);
            }

            return node;
        }
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.Combine("UploadedFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
        public string GetFileSizeInKilobytes(IFormFile file)
        {
            double fileSizeInBytes = file.Length;
            double fileSizeInKilobytes = fileSizeInBytes / 1024; // 1 килобайт = 1024 байта
            return fileSizeInKilobytes.ToString("N2"); // Округляем до 2 знаков после запятой и возвращаем строку
        }

        public string GetFullName(string shortName)
        {
            return ElementStringRepresentations
                .SingleOrDefault(s => s.ShortName == shortName)
                ?.FullName ?? shortName;
        }

        public XmlDocument ConvertJsonToXml(string jsonFilePath)
        {
            string jsonString = System.IO.File.ReadAllText(jsonFilePath);

            var xmlDocument = new XmlDocument();
            using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), XmlDictionaryReaderQuotas.Max))
            {
                xmlDocument.Load(jsonReader);
            }
            return xmlDocument;
        }

        public async Task<Node> FindNodeAsync(IEnumerable<Node> nodes, string buttonText)
        {
            foreach (var node in nodes)
            {
                if (buttonText == node.ShortName)
                {
                    return node;
                }
                var childNode = await FindNodeAsync(node.ChildNodes, buttonText);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }
    }
}

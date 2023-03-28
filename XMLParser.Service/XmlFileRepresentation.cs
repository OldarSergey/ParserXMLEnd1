using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XMLParser.Model;

namespace XMLParser.Service
{
    public class XmlFileRepresentation : IXmlRepository
    {
        private XmlDocument _xmlDocument;
        private List<ElementStringRepresentation> _elementStringRepresentations;
        private List<Node> _nodes;
        private Node _selectedNode;

        /// <summary>
        /// add check
        /// </summary>
        public XmlDocument XmlDocument 

        { get => _xmlDocument; 

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
        public Node SelectedNode { get => _selectedNode; set => _selectedNode = value; }
        public string FilePath { get; set; }

     

        //по дефолту переводит регистр
        public XmlFileRepresentation()
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
                var a = new Model.Attribute(node, item.Value, GetFullName(item.Name), item.Value);
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

      
        public Node TreeView(Node nodes)
        { 
            var childCount = nodes.ChildNodes?.Count ?? 0;
            if (childCount == 1)
                return nodes;
            
            for(int i = 0; i< childCount;i++)
            {
                var item = nodes.ChildNodes[i];
                var n = TreeView(item);
                Console.WriteLine(n);
            }
            return nodes;

            
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
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

        public string GetFullName(string shortName)
        {
            return ElementStringRepresentations
                .SingleOrDefault(s => s.ShortName == shortName)
                ?.FullName ?? shortName;
        }
    }
}

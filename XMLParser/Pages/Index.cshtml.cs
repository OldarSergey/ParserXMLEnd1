using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using XMLParser.Model;
using XMLParser.Service;

namespace XMLParser.Pages
{
    public class IndexModel : PageModel
    {

        public XmlFileRepresentation _xmlFile;

        private readonly Service.IXmlRepository _xmlRepository;

        public IndexModel(Service.IXmlRepository xmlRepository)
        {
            _xmlFile = new();
            _xmlRepository = xmlRepository;
        }
        public IEnumerable<Node> Node { get; set; }




        public IActionResult OnGet()
        {
            return Page();
        }

        [HttpPost]
        public  IActionResult OnPost(XmlFileRepresentation xmlFileRepresentation)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(xmlFileRepresentation.FilePath);

            Node = (IEnumerable<Node>)_xmlFile.GetRootNode(doc);
            
            _xmlFile.Nodes.Add((Node)Node);

            return Page();
        }

        


    }
}
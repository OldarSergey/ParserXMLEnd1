using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
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

       
        public  IActionResult OnPost(IFormFile file)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(UnicodeEncoding.UTF8.getString(new StringReader(file.FileName)));

            Node = (IEnumerable<Node>)_xmlFile.GetRootNode(doc);

            _xmlFile.Nodes.Add((Node)Node);

            return Page();
        }

        


    }
}
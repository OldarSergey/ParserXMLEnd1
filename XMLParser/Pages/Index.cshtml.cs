using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using XMLParser.Model;

namespace XMLParser.Pages
{
    public class IndexModel : PageModel
    {



        private readonly Service.IXmlRepository _xmlRepository;

        public IndexModel(Service.IXmlRepository xmlRepository)
        {

            _xmlRepository = xmlRepository;
        }

        [BindProperty]
        public IEnumerable<Node> Node { get; set; }



        
        public IActionResult OnGet()
        {
          
            return Page();

        }






        [HttpPost]
        public async Task<PartialViewResult> OnPost(IFormFile file)
        {
            try
            {
                if (await _xmlRepository.UploadFile(file))
                {
                    ViewData["Message"] = "File Upload Successful";
                }
                else
                {
                    ViewData["Message"] = "File Upload Failed";
                }
            }
            catch (Exception)
            {

                ViewData["Message"] = "File Upload Failed";
            }

            _xmlRepository.FilePath = (@$"C:\Users\Oldar\source\repos\XMLParser\XMLParser\UploadedFiles\{file.FileName}");

            _xmlRepository.XmlDocument = new XmlDocument();

           



           foreach(var item in _xmlRepository.Nodes)
           {

                System.Diagnostics.Debug.WriteLine(item);


                TreeView(item);
           }


            return Partial("_CarPartial",_xmlRepository.Nodes);

        }

        public Node TreeView(Node nodes)
        {
           
            var childCount = nodes.ChildNodes?.Count ?? 0;
            if (childCount < 1)
                return nodes;

            for (int i = 0; i < childCount; i++)
            {

                var item = nodes.ChildNodes[i];
                System.Diagnostics.Debug.WriteLine(item);
                TreeView(item);         
            }
            return nodes;


        }
    }
}
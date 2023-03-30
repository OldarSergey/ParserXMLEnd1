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
            

            _xmlRepository.FilePath = (@$"C:\Users\student\Source\Repos\XMLParser\XMLParser\UploadedFiles\{file.FileName}");

            _xmlRepository.XmlDocument = new XmlDocument();


            return Partial("_CarPartial",_xmlRepository.Nodes);

        }

       
    }
}
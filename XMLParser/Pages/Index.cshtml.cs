using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
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
        public IEnumerable<Node> Node { get; set; }




        public IActionResult OnGet()
        {
            return Page();

        }


        [HttpPost]
        public async Task<IActionResult> OnPost(IFormFile file)
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



            return Partial("_OutputPartial ");

        }




    }
}
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

        [BindProperty]
        public IEnumerable<Node> Node { get; set; }


        [FromRoute]
        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }



        public IActionResult OnGet()
        {

            return Page();

        }

        public PartialViewResult OnGetBtnClick()
        {
            foreach (var item in _xmlRepository.Nodes)
            {
                if(Message == item.ShortName)
                {
                    return Partial("_CarPartial", item.Attributes);
                }
                else
                {
                    return Partial ("_CarPartial", TreeView(item).Attributes);
                }
            }
            return null;
          


        }
        public Node TreeView(Node nodes)
        {
            var childCount = nodes.ChildNodes?.Count ?? 0;
            if (childCount < 1)
                return nodes;
            for(int i=0; i<childCount; i++)
            {
                if (Message == nodes.ChildNodes[i].ShortName)
                {
                    return nodes.ChildNodes[i];
                    
                }
                else
                {
                    TreeView(nodes.ChildNodes[i]);
                }
            }
            return nodes;
        }





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


            _xmlRepository.FilePath = (@$"C:\Users\student\source\repos\XMLParser\XMLParser\UploadedFiles\{file.FileName}");

            _xmlRepository.XmlDocument = new XmlDocument();



            return Partial("_CarPartial", _xmlRepository.Nodes);

        }


    }
}
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
        public string ButtonText { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public PartialViewResult OnGetBtnClick(string buttonText)
        {
			var dtos = new List<DTOXmlRepresentation>();
			foreach (var item in _xmlRepository.Nodes)
            {
                if (buttonText == item.ShortName)
                {
                    dtos.Add(new DTOXmlRepresentation(item.ShortName, item.FullName, item.Attributes));
					return Partial("_AttributeRepresentation", dtos);
                }
                    
            }

            var node = FindNode(_xmlRepository.Nodes, buttonText);

            if (node != null)
            {
                dtos.Add(new DTOXmlRepresentation(node.ShortName, node.FullName, node.Attributes));
            }

			if (dtos.Any())
			{
				return Partial("_AttributeRepresentation", dtos);
			}
            return null;
		}

       

        private Node FindNode(IEnumerable<Node> nodes, string buttonText)
        {
            foreach (var node in nodes)
            {
                if (buttonText == node.ShortName)
                {
                    return node;
                }
                var childNode = FindNode(node.ChildNodes, buttonText);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }





        public async Task<PartialViewResult> OnPost(IFormFile file)
        {
            if (file != null)
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


                _xmlRepository.FilePath = (@$"C:\Users\Oldar\Source\Repos\XMLParser\XMLParser\UploadedFiles\{file.FileName}");

                _xmlRepository.XmlDocument = new XmlDocument();



                return Partial("_CarPartial", _xmlRepository.Nodes);
            }

            else {
                return Partial("Empty");
            }
            


        }


    }
}
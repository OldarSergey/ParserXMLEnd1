using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParserXml.Data;
using ParserXml.Model;
using ParserXml.Model.DTO;
using ParserXml.Model.EntitiesDbContext;
using ParserXml.Service;
using System.Data;
using System.Xml;
using File = ParserXml.Model.EntitiesDbContext.File;

namespace ParserXml.Pages
{
    public class UserAuthorizationModelModel : PageModel
    {
        private readonly IXmlFileParser _xmlRepository;
        private readonly ILoggingsService _loggingsService;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserAuthorizationModelModel
        (   
            IXmlFileParser xmlRepository, 
            ILoggingsService loggingsService, 
            IFileService fileService, 
            UserManager<User> userManager,
            ApplicationDbContext context
        )
        {
            _xmlRepository = xmlRepository;
            _loggingsService = loggingsService;
            this._fileService = fileService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IEnumerable<Node> Node { get; set; }
        [FromRoute]
        [BindProperty(SupportsGet = true)]
        public string FileName { get; set; }

        public void OnGet()
        {
        }

        public async Task<PartialViewResult> OnGetBtnClick(string buttonText)
        {
            var dtos = new List<DTOXmlRepresentation>();
            foreach(var item in _xmlRepository.Nodes)
            {
                if (buttonText == item.ShortName)
                {
                    dtos.Add(new DTOXmlRepresentation(item.ShortName, item.FullName, item.Attributes));
                    return Partial("_AttributeRepresentation", dtos);
                }
            }
            var node = await _xmlRepository.FindNodeAsync(_xmlRepository.Nodes, buttonText);
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
        public async Task<PartialViewResult> OnPost(IFormFile file)
        {
            if (file == null)
                return Partial("Empty");
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (await _xmlRepository.UploadFile(file))
                {
                    FileName = file.FileName;
                    string basePath = Path.Combine(Environment.CurrentDirectory, "UploadedFiles");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string relativePath = Path.Combine("UploadedFiles", fileName);
                    string fullPath = Path.Combine(basePath, fileName);

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var newFile = new File
                    {
                        FileName = file.FileName,
                        FilePath = relativePath,
                        FileExtension = Path.GetExtension(file.FileName),
                        VolumeFile = _xmlRepository.GetFileSizeInKilobytes(file),
                        UserId = user.Id
                    };
                    await _fileService.AddFileAsync(newFile);
                    var newLog = new Logging
                    {
                        Date = DateTime.Now,
                        UserId = user.Id,
                        FileId = newFile.Id
                    };
                    await _loggingsService.AddLoggingAsync(newLog);

                    _xmlRepository.FilePath = relativePath;
                    _xmlRepository.XmlDocument = new XmlDocument();

                    return Partial("_TreeViewPartial", _xmlRepository.Nodes);
                }
            }

            return Partial("_TreeViewPartial");
        }
        public PartialViewResult OnGetRepeateFileParser(string buttonText)
        {
            var file = _context.Files.FirstOrDefault(file => file.FileName == buttonText);

            if (file != null)
            {
                _xmlRepository.FilePath = _fileService.FindFilePath(buttonText);
                _xmlRepository.XmlDocument = new XmlDocument();
                return Partial("_TreeViewPartial", _xmlRepository.Nodes);
            }

            return Partial("_TreeViewPartial");
        }
        public async Task<List<Logging>> GetLoggingsByUserIdAsync()
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);
            var userId = user.Id;

            if (User.IsInRole("Администратор"))
            {
                return await _context.Loggings
                    .Include(log => log.File)
                    .Include(log => log.User)
                    .Where(log => !log.File.IsDeleted)
                    .ToListAsync();
            }
            else
            {
                return await _context.Loggings
                    .Where(logging => logging.UserId == userId && !logging.File.IsDeleted)
                    .Include(log => log.File)
                    .ToListAsync();
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OnPostDeleteFile(int fileId)
        {
            await _fileService.DeleteFileAsync(fileId);

            return RedirectToAction("UserAuthorizationModel");
        }
    }
}

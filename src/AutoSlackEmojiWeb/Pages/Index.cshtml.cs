using AutoSlackEmoji.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoSlackEmojiWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHostEnvironment _environment;
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IHostEnvironment environment, IFileRepository fileRepository, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

        [BindProperty]
        public IFormFile UploadContent { get; set; }

        public async Task OnPostAsync()
        {
            if (UploadContent != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", UploadContent.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    await UploadContent.CopyToAsync(memoryStream);
                    await _fileRepository.AddFileAsync(memoryStream, UploadContent.FileName, _configuration["aws_bucket_name"] ?? "happyday");
                }

            }
        }
        
        public void OnGet()
        {

        }
    }
}
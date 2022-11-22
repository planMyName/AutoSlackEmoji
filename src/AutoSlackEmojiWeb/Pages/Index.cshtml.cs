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
        private readonly ISlackClient _slackClient;

        public IndexModel(ILogger<IndexModel> logger, IHostEnvironment environment, IFileRepository fileRepository, IConfiguration configuration, ISlackClient slackClient)
        {
            _logger = logger;
            _environment = environment;
            _fileRepository = fileRepository;
            _configuration = configuration;
            _slackClient = slackClient;
        }

        [BindProperty]
        public IFormFile UploadContent { get; set; }

        [BindProperty]
        public string EmojiName { get; set; }

        public async Task OnPostAsync()
        {
            if (UploadContent != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", UploadContent.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    // receive file
                    await UploadContent.CopyToAsync(memoryStream);
                    // process image
                    
                    // upload post process result to S3
                    var preSignedFileUrl = await _fileRepository.AddFileAsync(memoryStream, EmojiName, _configuration["aws_bucket_name"] ?? "happyday");

                    // add emoji to slack
                    await _slackClient.AddEmojiAsync(EmojiName, preSignedFileUrl);
                }

            }
        }
        
        public void OnGet()
        {

        }
    }
}
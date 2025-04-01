using HtmlToMarkdownService;
using Microsoft.AspNetCore.Mvc;
namespace HtmlToMarkdown.App.Controller
{
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        [HttpPost ("ConvertHtmlToMarkdown")]
        [RequestSizeLimit(1024 * 1024)] // Limit request size to 1 MB        
        public IActionResult ConvertHtmlToMarkdown([FromBody] HtmlInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Html))
            {
                return BadRequest("Input HTML cannot be empty.");
            }
            try
            {
                var conversionService = new ConversionService();
                string markdownContent = conversionService.ConvertHtmlToMarkdown(input.Html);
                var errorLogs = conversionService.GetErrorLogs();

                return new JsonResult(new { markdown = markdownContent, errors = string.Join(", ", errorLogs) });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during conversion.");
            }
        }

        public class HtmlInputModel
        {
            public string? Html { get; set; }
        }
    }
}

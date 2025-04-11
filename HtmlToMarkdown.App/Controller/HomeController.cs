using HtmlToMarkdown.Service;
using Microsoft.AspNetCore.Mvc;
namespace HtmlToMarkdown.App.Controller
{
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        [HttpPost("ConvertHtmlToMarkdown")]
        [RequestSizeLimit(1024 * 1024)] // Limit request size to 1 MB        
        public IActionResult ConvertHtmlToMarkdown([FromBody] HtmlInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Html))
            {
                return BadRequest("Input HTML cannot be empty.");
            }
            try
            {
                var result = ConversionService.ConvertHtmlToMarkdown(input.Html);
                return new JsonResult(new { markdown = result.Markdown, errors = string.Join(", ", result.Errors) });
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

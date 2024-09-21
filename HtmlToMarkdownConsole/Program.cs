
namespace HtmlToMarkdownConsole
{
    public class Program
    {

        static void Main(string[] args)
        {
            try
            {
                ConversionService conversionService = new ConversionService();
                string html = @"<h1>Document Title</h1>
<p>This is a paragraph with <a href='https://example.com'>a link</a>, some <b>bold</b> text, and some <i>italic</i> text.</p>

<ul>
  <li>First list item</li>
  <li>Second list item with <a href='https://example.com'>a link</a></li>
</ul>

<blockquote>This is a blockquote with <a href='https://example.com'>a link</a> and <code>inline code</code>.</blockquote>

<ol>
  <li>First ordered item</li>
  <li>Second ordered item</li>
</ol>

<table>
  <tr>
    <th>Header 1</th>
    <th>Header 2</th>
  </tr>
  <tr>
    <td>Data 1</td>
    <td>Data 2</td>
  </tr>
</table>

<table>
  <thead>
    <tr>
      <th>Header 1</th>
      <th>Header 3</th>
      <th>Header 2</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Data 1</td>
      <td>Data 3</td>
      <td>Data 2</td>
    </tr>
  </tbody>
</table>


<form>
  <input type='text' placeholder='Enter text' />
  <button>Submit</button>
</form>";

                var result = conversionService.ConvertHtmlToMarkdown(html);
                Console.WriteLine("Markdown Output:");
                Console.WriteLine(result);
                var errors = conversionService.GetErrorLogs();
                if (errors.Any())
                {
                    errors.ForEach(error =>
                    {
                        Console.WriteLine(error);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
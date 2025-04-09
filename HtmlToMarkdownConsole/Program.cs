
using BenchmarkDotNet.Running;
using HtmlToMarkdown.Service;

namespace HtmlToMarkdownConsole
{
    public class Program
    {

        static void Main(string[] args)
        {
            try
            {
#if BENCHMARK
                var summary = BenchmarkRunner.Run<HtmlToMarkdownBenchmark>();
#endif
                string html = @"

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

<ul><li>Item 1<ol><li>Item 1</li><li>Item 2</li><li>Item 3</li></ol></li><li>Item 2</li><li>Item 3</li></ul>
";

                var result = ConversionService.ConvertHtmlToMarkdown(html);
                Console.WriteLine("Markdown Output:");
                Console.WriteLine(result.Markdown);
                var errors = result.Errors;
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
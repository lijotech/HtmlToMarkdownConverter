using BenchmarkDotNet.Attributes;
using HtmlToMarkdown.Service;
public class HtmlToMarkdownBenchmark
{
    private string html100;
    private string html500;
    private string html1000;
    private string html5000;
    private string html10000;

    public HtmlToMarkdownBenchmark()
    {
        // Generate sample HTML strings of varying sizes
        html100 = GenerateSampleHtml(100);
        html500 = GenerateSampleHtml(500);
        html1000 = GenerateSampleHtml(1000);
        html5000 = GenerateSampleHtml(5000);
        html10000 = GenerateSampleHtml(10000);
    }

    // Method to benchmark the conversion of 100 characters of HTML to Markdown
    [Benchmark]
    public void ConvertHtml100()
    {
        ConversionService.ConvertHtmlToMarkdown(html100);
    }

    // Method to benchmark the conversion of 500 characters of HTML to Markdown
    [Benchmark]
    public void ConvertHtml500()
    {
        ConversionService.ConvertHtmlToMarkdown(html500);
    }

    // Method to benchmark the conversion of 1000 characters of HTML to Markdown
    [Benchmark]
    public void ConvertHtml1000()
    {
        ConversionService.ConvertHtmlToMarkdown(html1000);
    }

    // Method to benchmark the conversion of 5000 characters of HTML to Markdown
    [Benchmark]
    public void ConvertHtml5000()
    {
        ConversionService.ConvertHtmlToMarkdown(html5000);
    }

    // Method to benchmark the conversion of 10000 characters of HTML to Markdown
    [Benchmark]
    public void ConvertHtml10000()
    {
        ConversionService.ConvertHtmlToMarkdown(html10000);
    }

    // Generate sample HTML with specified character length
    private string GenerateSampleHtml(int length)
    {
        string sampleHtml = "<html><body><p>This is sample text for HTML conversion</p>";

        // Add more content until reaching the desired length
        while (sampleHtml.Length < length)
        {
            sampleHtml += "<p>Another sample paragraph to reach the length.</p>";
        }

        sampleHtml += "</body></html>";
        return sampleHtml.Substring(0, length);
    }
}


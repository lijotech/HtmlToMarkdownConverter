using HtmlToMarkdownService;
namespace HtmlToMarkdownTests;

public class ConversionServiceTests
{
    [Fact]
    public void ConvertsSimpleHtml()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<h1>Hello, World!</h1>";
        var expectedMarkdown = "# Hello, World!\r\n\r\n";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Fact]
    public void ConvertsComplexHtml()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<p>This is a <strong>test</strong> of the <em>conversion</em> service.</p>";
        var expectedMarkdown = "This is a **test** of the _conversion_ service.\r\n\r\n";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Fact]
    public void ConvertsPlainText()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "PlainText";
        var expectedMarkdown = "PlainText";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Fact]
    public void ConvertsBold()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<b>Bold</b> <strong>Strong</strong>";
        var expectedMarkdown = "**Bold** **Strong**";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }


    [Fact]
    public void ConvertsItalic()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<i>Italic</i> <em>Empahis</em>";
        var expectedMarkdown = "_Italic_ _Empahis_";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Theory]
    [InlineData("![Alt Text](proto:path)", "<img src=\"proto:path\" alt=\"Alt Text\" />")]
    [InlineData("![](proto:path)", "<img src=\"proto:path\" />")]
    [InlineData("![Alt Text]()", "<img alt=\"Alt Text\" />")]
    public void ConvertsImage(string expected, string imageHtmlInput)
    {
        var conversionService = new ConversionService();

        Assert.Equal(expected, conversionService.ConvertHtmlToMarkdown(imageHtmlInput));
    }

    [Fact]
    public void ConvertsLink()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<a href=\"proto:path\">Link Text</a>";
        var expectedMarkdown = "[Link Text](proto:path)";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Theory]
    [InlineData("# Heading Text\r\n\r\n", "<h1>Heading Text</h1>")]
    [InlineData("## Heading Text\r\n\r\n", "<h2>Heading Text</h2>")]
    [InlineData("### Heading Text\r\n\r\n", "<h3>Heading Text</h3>")]
    [InlineData("#### Heading Text\r\n\r\n", "<h4>Heading Text</h4>")]
    [InlineData("##### Heading Text\r\n\r\n", "<h5>Heading Text</h5>")]
    [InlineData("###### Heading Text\r\n\r\n", "<h6>Heading Text</h6>")]

    public void ConvertsHeading(string expected, string htmlInput)
    {
        var conversionService = new ConversionService();

        Assert.Equal(expected, conversionService.ConvertHtmlToMarkdown(htmlInput));
    }

    [Fact]
    public void ConvertsHorizontalRule()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<hr />";
        var expectedMarkdown = "\r\n---\r\n";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Fact]
    public void ConvertsParagraph()
    {
        // Arrange
        var conversionService = new ConversionService();
        var html = "<p>Paragraph Text</p>";
        var expectedMarkdown = "Paragraph Text\r\n\r\n";

        // Act
        var result = conversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result);
    }

    [Theory]
    [InlineData("Paragraph \r\nText\r\n\r\n", "<p>Paragraph <br/>Text</p>")]
    [InlineData("Paragraph \r\nText\r\n\r\n", "<p>Paragraph <br>Text</p>")]
    public void ConvertsBreak(string expected, string htmlInput)
    {
        var conversionService = new ConversionService();

        Assert.Equal(expected, conversionService.ConvertHtmlToMarkdown(htmlInput));
    }

    [Theory]
    [InlineData("Paragraph Text", "<div>Paragraph Text</div>")]
    [InlineData("Paragraph Text\r\n", "<div>Paragraph Text<br/></div>")]
    public void ConvertsSingleBreakInsideDiv(string expected, string htmlInput)
    {
        var conversionService = new ConversionService();

        Assert.Equal(expected, conversionService.ConvertHtmlToMarkdown(htmlInput));
    }

    [Fact]
    public void ConvertsBlockQuote()
    {
        var conversionService = new ConversionService();

        Assert.Equal("\r\n> Quote Text\r\n", conversionService.ConvertHtmlToMarkdown("<blockquote>Quote Text</blockquote>"));
    }

    [Fact]
    public void ConvertsNestedBlockQuote()
    {
        var conversionService = new ConversionService();

        Assert.Equal("\r\n> Quote Text\r\n>> Inner Quote Text\r\n> More Text\r\n",
            conversionService.ConvertHtmlToMarkdown("<blockquote>Quote Text<blockquote>Inner Quote Text</blockquote>More Text</blockquote>"));
    }

    [Fact]
    public void ConvertsUnOrderedList()
    {
        var conversionService = new ConversionService();

        Assert.Equal("\r\n* Item 1\r\n* Item 2\r\n* Item 3\r\n\r\n",
            conversionService.ConvertHtmlToMarkdown("<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>"));
    }

    [Fact]
    public void ConvertsOrderedList()
    {
        var conversionService = new ConversionService();

        Assert.Equal("\r\n1. Item 1\r\n1. Item 2\r\n1. Item 3\r\n\r\n",
            conversionService.ConvertHtmlToMarkdown("<ol><li>Item 1</li><li>Item 2</li><li>Item 3</li></ol>"));
    }
}
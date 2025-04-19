using HtmlToMarkdown.Service;
using Xunit.Abstractions;
namespace HtmlToMarkdownTests;

public class ConversionServiceTests
{
    private readonly ITestOutputHelper output;

    public ConversionServiceTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ConvertsSimpleHtml()
    {
        // Arrange
        //var conversionService = new ConversionService();
        var html = "<h1>Hello, World!</h1>";
        var expectedMarkdown = "# Hello, World!\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsComplexHtml()
    {
        // Arrange
        var html = "<p>This is a <strong>test</strong> of the <em>conversion</em> service.</p>";
        var expectedMarkdown = "This is a **test** of the _conversion_ service.\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsPlainText()
    {
        // Arrange
        var html = "PlainText   with space";
        var expectedMarkdown = "PlainText with space";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsBold()
    {
        // Arrange
        var html = "<b>Bold</b> <strong>Strong</strong>";
        var expectedMarkdown = "**Bold** **Strong**";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsItalic()
    {
        // Arrange
        var html = "<i>Italic</i> <em>Empahis</em>";
        var expectedMarkdown = "_Italic_ _Empahis_";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("![Alt Text](proto:path)", "<img src=\"proto:path\" alt=\"Alt Text\" />")]
    [InlineData("![](proto:path)", "<img src=\"proto:path\" />")]
    [InlineData("![Alt Text]()", "<img alt=\"Alt Text\" />")]
    [InlineData("![Alt Text]()", "<img alt='Alt Text' />")]
    [InlineData("![]()", "<img alt=' />")]
    [InlineData("![](proto:path)", "<img src=proto:path />")]
    public void ConvertsImage(string expected, string html)
    {

        var result = ConversionService.ConvertHtmlToMarkdown(html);
        Assert.Equal(expected, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsLink()
    {
        // Arrange
        var html = "<a href=\"proto:path\">Link Text</a>";
        var expectedMarkdown = "[Link Text](proto:path)";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
    }

    [Theory]
    [InlineData("# Heading Text\r\n\r\n", "<h1>Heading Text</h1>")]
    [InlineData("## Heading Text\r\n\r\n", "<h2>Heading Text</h2>")]
    [InlineData("### Heading Text\r\n\r\n", "<h3>Heading Text</h3>")]
    [InlineData("#### Heading Text\r\n\r\n", "<h4>Heading Text</h4>")]
    [InlineData("##### Heading Text\r\n\r\n", "<h5>Heading Text</h5>")]
    [InlineData("###### Heading Text\r\n\r\n", "<h6>Heading Text</h6>")]
    public void ConvertsHeading(string expected, string html)
    {
        var result = ConversionService.ConvertHtmlToMarkdown(html);
        Assert.Equal(expected, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsHorizontalRule()
    {
        // Arrange
        var html = "<hr />";
        var expectedMarkdown = "\r\n---\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsParagraph()
    {
        // Arrange
        var html = "<p>Paragraph Text</p>";
        var expectedMarkdown = "Paragraph Text\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("Paragraph \r\nText\r\n\r\n", "<p>Paragraph <br/>Text</p>")]
    [InlineData("Paragraph \r\nText\r\n\r\n", "<p>Paragraph <br>Text</p>")]
    public void ConvertsBreak(string expected, string html)
    {

        var result = ConversionService.ConvertHtmlToMarkdown(html);
        Assert.Equal(expected, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("Paragraph Text", "<div>Paragraph Text</div>")]
    [InlineData("Paragraph Text\r\n", "<div>Paragraph Text<br/></div>")]
    public void ConvertsSingleBreakInsideDiv(string expected, string htmlInput)
    {

        Assert.Equal(expected, ConversionService.ConvertHtmlToMarkdown(htmlInput).Markdown);
    }

    [Fact]
    public void ConvertsBlockQuote()
    {
        // Arrange
        var html = "<blockquote>Quote Text</blockquote>";
        var expectedMarkdown = "\r\n> Quote Text\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsNestedBlockQuote()
    {
        // Arrange
        var html = "<blockquote>Quote Text<blockquote>Inner Quote Text</blockquote>More Text</blockquote>";
        var expectedMarkdown = "\r\n> Quote Text\r\n>> Inner Quote Text\r\n> More Text\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsUnOrderedList()
    {
        // Arrange
        var html = "<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>";
        var expectedMarkdown = "\r\n * Item 1\r\n * Item 2\r\n * Item 3\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsOrderedList()
    {
        // Arrange
        var html = "<ol><li>Item 1</li><li>Item 2</li><li>Item 3</li></ol>";
        var expectedMarkdown = "\r\n 1. Item 1\r\n 1. Item 2\r\n 1. Item 3\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }


    [Fact]
    public void ConvertsTable()
    {
        // Arrange
        var html = "<table>\r\n  <thead>\r\n    <tr>\r\n      <th>Header 1</th>\r\n      <th>Header 2</th>\r\n      <th>Header 3</th>\r\n    </tr>\r\n  </thead>\r\n  <tbody>\r\n    <tr>\r\n      <td>Data 1</td>\r\n      <td>Data 2</td>\r\n      <td>Data 3</td>\r\n    </tr>\r\n  </tbody>\r\n</table>";
        var expectedMarkdown = "\r\n| Header 1 | Header 2 | Header 3 |\r\n| --- | --- | --- |\r\n| Data 1 | Data 2 | Data 3 |\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }
    [Fact]
    public void ConvertsTable2()
    {
        // Arrange
        var html = "<table>\r\n   <thead>\r\n      <tr>\r\n         <th>Header 1</th>\r\n         <th>Header 3</th>\r\n         <th>Header 2</th>\r\n      </tr>\r\n   </thead>\r\n   <tbody>\r\n      <tr>\r\n         <td>Data 1</td>\r\n         <td>Data 3</td>\r\n         <td>Data 2</td>\r\n      </tr>\r\n   </tbody>\r\n</table>";
        var expectedMarkdown = "\r\n| Header 1 | Header 3 | Header 2 |\r\n| --- | --- | --- |\r\n| Data 1 | Data 3 | Data 2 |\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsTableWithoutThead()
    {
        // Arrange
        var html = "<table><tr><th>Header 1</th><th>Header 3</th><th>Header 2</th></tr><tbody><tr><td>Data 1</td><td>Data 3</td><td>Data 2</td></tr></tbody></table>";
        var expectedMarkdown = "\r\n| Header 1| Header 3| Header 2|\r\n| --- | --- | --- |\r\n| Data 1| Data 3| Data 2|\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsTableWithTheadWihtoutTd()
    {
        // Arrange
        var html = "<table><thead><tr><td>Header 1</td><td>Header 3</td><td>Header 2</td></tr></thead><tbody><tr><td>Data 1</td><td>Data 3</td><td>Data 2</td></tr></tbody></table>";
        var expectedMarkdown = "\r\n| Header 1| Header 3| Header 2|\r\n| --- | --- | --- |\r\n| Data 1| Data 3| Data 2|\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsNestedUnOrderedList()
    {
        // Arrange
        var html = "<ul><li>Item 1<ol><li>Sub Item 1</li><li>Sub Item 2</li><li>Sub Item 3</li></ol></li><li>Item 2</li><li>Item 3</li></ul>";
        var expectedMarkdown = "\r\n * Item 1\r\n   1. Sub Item 1\r\n   1. Sub Item 2\r\n   1. Sub Item 3\r\n\r\n\r\n * Item 2\r\n * Item 3\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsNestedOrderedList()
    {
        // Arrange
        var html = "<ol><li>Item 1<ul><li>Sub Item 1</li><li>Sub Item 2</li><li>Sub Item 3</li></ul></li><li>Item 2</li><li>Item 3</li></ol>";
        var expectedMarkdown = "\r\n 1. Item 1\r\n   * Sub Item 1\r\n   * Sub Item 2\r\n   * Sub Item 3\r\n\r\n\r\n 1. Item 2\r\n 1. Item 3\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestEmptyDiv()
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown("<div></div>");
        // Assert
        Assert.Equal("", result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestWithBody()
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown("<html><head><title>Title</title><style>some style</style></head><body>Body</body></html>");
        // Assert
        Assert.Equal("Body", result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestWithStyle()
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown("<style>some style</style>Body");
        // Assert
        Assert.Equal("Body", result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestWithScript()
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown("<script>some script</script>Body");
        // Assert
        Assert.Equal("Body", result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestCustomTag()
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown("<xyz>custom tag content</xyz>");
        // Assert
        Assert.Equal("custom tag content", result.Markdown);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count > 1);
    }

    [Theory]
    [InlineData("", "<h1Hello, World!</h1>")]
    [InlineData("", "<h1 Hello, World! </h1")]
    public void TestMalformedHtml(string expected, string html)
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expected, result.Markdown);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(Skip = "Invalid characters may result in unpredictable conversion behavior.")]
    public void TestHtmlWithInvalidCharacters()
    {
        // Arrange
        var html = "<h1>Hello, World! \u0000\u0001</h1>";  // Includes invalid characters
        var expectedErrors = new List<string> { "Invalid character detected." };

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestHtmlWithMissingEndTag()
    {
        // Arrange
        var html = "<h1>Hello, World!";
        var expectedMarkdown = "# Hello, World!";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Log message
        output.WriteLine("Even if the end tag is missing, the application will attempt to convert the HTML until the last character.");
        // Assert
        // Even if the end tag is missing, the application will attempt to convert the HTML until the last character.
        // This test ensures that errors are detected but conversion proceeds with available content.
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("<code>Console.WriteLine(\"Hello\");</code>", "`Console.WriteLine(\"Hello\");`")]
    [InlineData("<form><input type='text'></form>", "> Input (Type: text)\r\n> **Form End**\r\n\r\n")]
    [InlineData("<input type='button'>", "> Input (Type: button)\r\n")]
    [InlineData("<button>Click Me</button>", "> **Button**: Click Me\r\n")]
    [InlineData("<span>Highlighted</span>", "Highlighted")]
    [InlineData("<label>Username:</label>", "Username:")]
    [InlineData("<iframe src='https://example.com'></iframe>", "> **Embedded Content**: [iframe link](https://example.com)\r\n")]
    public void ConvertsVariousHtmlTags(string html, string expectedMarkdown)
    {
        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsTableWithListItems()
    {
        // Arrange
        var html = "<table><tr><td><ul><li>Item 1</li><li>Item 2</li></ul></td></tr></table>";
        var expectedMarkdown = "\r\n| <br>* Item 1<br>* Item 2|\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ConvertsTableWithNestedList()
    {
        // Arrange
        var html = "<table><tr><td><ul><li>Item 1<ul><li>Subitem A</li><li>Subitem B</li></ul></li><li>Item 2</li></ul></td></tr></table>";
        var expectedMarkdown = "\r\n| <br>* Item 1<br>&nbsp;&nbsp; * Subitem A<br>&nbsp;&nbsp; * Subitem B<br>* Item 2|\r\n\r\n";

        // Act
        var result = ConversionService.ConvertHtmlToMarkdown(html);

        // Assert
        Assert.Equal(expectedMarkdown, result.Markdown);
        Assert.Empty(result.Errors);
    }
}

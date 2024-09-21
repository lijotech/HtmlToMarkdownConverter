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
}
# HTML to Markdown Converter

**HTML to Markdown Converter** is a powerful tool designed to seamlessly convert HTML strings into Markdown format.  This library has no external dependencies, making it lightweight and easy to integrate into your projects. This tool simplifies the process of transforming HTML content into clean, readable Markdown, making it perfect for developers, writers, and content creators.

## Features

- **Easy Conversion**: Quickly convert HTML strings to Markdown with minimal effort.
- **Accurate Formatting**: Ensures that the converted Markdown retains the structure and style of the original HTML.
- **Open Source**: Available for free and open for contributions.

## Installation

To install the tool, use the following command:

```Bash
dotnet add package HtmlToMarkdownConverter

```

## Breaking Changes in v2.0.0

### Updated Usage (v2.0.0)

```csharp
string htmlContent = "<p>Hello, World!</p>";
var result = ConversionService.ConvertHtmlToMarkdown(htmlContent);
string outputString = result.Markdown; 
List<string> errorLogs = result.Errors; 
```

### What Changed?

- Static Method Implementation:

	- The `ConversionService` is now replaced with Static class, offering a static method for improved simplicity.

	- The `ConvertHtmlToMarkdown` method now returns a Result object containing both the Markdown string and error logs.

- Result Object:

	- Instead of separate methods for `ConvertHtmlToMarkdown` and `GetErrorLogs`, these are now combined into a single static call returning a structured object.

- Namespace has been updated
	
	- From `v2.0.0` the namespace is `HtmlToMarkdown.Service` previously it was `HtmlToMarkdownService`.

### Previous Usage (<=v1.0.9)

```csharp
using HtmlToMarkdownService;

var conversionService = new ConversionService();
string htmlContent = "<p>Hello, World!</p>";
string markdownContent = conversionService.ConvertHtmlToMarkdown(htmlContent);
List<string> errorLogs = conversionService.GetErrorLogs();
```

## Usage

```csharp
var result = ConversionService.ConvertHtmlToMarkdown("<p>Hello, World!</p>");
Console.WriteLine(result.Markdown);
if (result.Errors.Any())
{
    Console.WriteLine("Conversion Errors:");
    result.Errors.ForEach(Console.WriteLine);
}
```

**ConversionService Class**

The ConversionService class provides methods to convert HTML to Markdown and to retrieve error logs.

**ConvertHtmlToMarkdown Method**

This method converts HTML content to Markdown.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## Contact

For any questions or feedback, please contact info@bluecomment.com.
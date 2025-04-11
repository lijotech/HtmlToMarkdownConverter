# HtmlToMarkdownConverter

![Version](https://img.shields.io/badge/version-2.0.0-blue)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)


HtmlToMarkdownConverter is a .NET library that converts HTML content to Markdown format. This library has no external dependencies, making it lightweight and easy to integrate into your projects. The repository includes the main library, a console application for testing, and a unit test project to verify supported conversions.

## Installation

To install the HtmlToMarkdownConverter NuGet package, use the following command in the Package Manager Console:

```Bash
dotnet add package HtmlToMarkdownConverter

```

## Breaking Changes in v2.0.0

### Previous Usage
```csharp
using HtmlToMarkdownService;

var conversionService = new ConversionService();
string htmlContent = "<p>Hello, World!</p>";
string markdownContent = conversionService.ConvertHtmlToMarkdown(htmlContent);
List<string> errorLogs = conversionService.GetErrorLogs();
```

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

## Console Application

A console application is provided to check the functionality of the HtmlToMarkdownConverter. You can find it in the following path:

[HtmlToMarkdownConsole](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownConsole)

## Unit Tests

To see which conversions are supported and to verify the functionality, refer to the unit test project available at:

[HtmlToMarkdownTests](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownTests)

## Reporting Issues

If you encounter any problems, please report them [here](https://github.com/lijotech/HtmlToMarkdownConverter/issues).

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
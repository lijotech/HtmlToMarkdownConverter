# HtmlToMarkdownConverter

![Version](https://img.shields.io/badge/version-2.0.0-blue)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)
![Coverage](https://img.shields.io/badge/coverage-90%25-brightgreen)


HtmlToMarkdownConverter is a .NET library that converts HTML content to Markdown format. This library has no external dependencies, making it lightweight and easy to integrate into your projects. The repository includes the main library, a console application for testing, and a unit test project to verify supported conversions.

## Installation

To install the HtmlToMarkdownConverter NuGet package, use the following command in the Package Manager Console:

```Bash
dotnet add package HtmlToMarkdownConverter

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

## Console Application

A console application is provided to check the functionality of the HtmlToMarkdownConverter. You can find it in the following path:

[HtmlToMarkdownConsole](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownConsole)

## Unit Tests

To see which conversions are supported and to verify the functionality, refer to the unit test project available at:

[HtmlToMarkdownTests](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownTests)

## Web application

Online conversion is supported using the web application developed using asp.net razor pages.

[Check Online](https://htmltomarkdownconverter.azurewebsites.net/)

## Reporting Issues

If you encounter any problems, please report them [here](https://github.com/lijotech/HtmlToMarkdownConverter/issues).

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
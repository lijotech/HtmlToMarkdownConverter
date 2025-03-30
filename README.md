# HtmlToMarkdownConverter

![Version](https://img.shields.io/badge/version-1.0.8-blue)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)


HtmlToMarkdownConverter is a .NET library that converts HTML content to Markdown format. This library has no external dependencies, making it lightweight and easy to integrate into your projects. The repository includes the main library, a console application for testing, and a unit test project to verify supported conversions.

## Installation

To install the HtmlToMarkdownConverter NuGet package, use the following command in the Package Manager Console:

```sh
Install-Package HtmlToMarkdownConverter

```

## Usage

After installing the package, you can use it in your project as follows:

```
using HtmlToMarkdownService;

var conversionService = new ConversionService();
string htmlContent = "<p>Hello, World!</p>";
string markdownContent = conversionService.ConvertHtmlToMarkdown(htmlContent);

Console.WriteLine(markdownContent);

```

## Console Application

A console application is provided to check the functionality of the HtmlToMarkdownConverter. You can find it in the following path:

[HtmlToMarkdownConsole](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownConsole)

## Unit Tests

To see which conversions are supported and to verify the functionality, refer to the unit test project available at:

[HtmlToMarkdownTests](https://github.com/lijotech/HtmlToMarkdownConverter/tree/master/HtmlToMarkdownTests)

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
# HTML to Markdown Converter

**HTML to Markdown Converter** is a powerful tool designed to seamlessly convert HTML strings into Markdown format. This tool simplifies the process of transforming HTML content into clean, readable Markdown, making it perfect for developers, writers, and content creators.

## Features

- **Easy Conversion**: Quickly convert HTML strings to Markdown with minimal effort.
- **Accurate Formatting**: Ensures that the converted Markdown retains the structure and style of the original HTML.
- **Open Source**: Available for free and open for contributions.

## Installation

To install the tool, use the following command:

```sh
Install-Package HtmlToMarkdownService

```

## Usage

**ConversionService Class**

The ConversionService class provides methods to convert HTML to Markdown and to retrieve error logs.

**ConvertHtmlToMarkdown Method**

This method converts HTML content to Markdown.

```
using HtmlToMarkdownService;

var conversionService = new ConversionService();
string htmlContent = "<p>Hello, World!</p>";
string markdownContent = conversionService.ConvertHtmlToMarkdown(htmlContent);

Console.WriteLine(markdownContent);
```

**GetErrorLogs Method**

This method returns a list of errors encountered during the conversion process.

```
using HtmlToMarkdownService;

var conversionService = new ConversionService();
List<string> errorLogs = conversionService.GetErrorLogs();

foreach (var error in errorLogs)
{
    Console.WriteLine(error);
}

```

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request.

## Contact
For any questions or feedback, please contact info@bluecomment.com.
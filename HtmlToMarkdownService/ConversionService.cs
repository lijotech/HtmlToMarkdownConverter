using System.Text;
using System.Text.RegularExpressions;

namespace HtmlToMarkdown.Service;

/// <summary>
/// Static service class to handle all conversion of HTML to Markdown efficiently.
/// </summary>
public static class ConversionService
{
    /// <summary>
    /// The contents of this tag are ignored for markdown generation
    /// </summary>
    private static readonly HashSet<string> _ignoredTags = new() { "head", "title", "style", "script" };
    /// <summary>
    /// Allowed html tags for conversion
    /// </summary>
    private static readonly HashSet<string> _allowedTags = new() { "div", "iframe", "input", "label", "button", "span", "i", "em", "br", "hr", "form", "tbody", "table", "thead", "tr", "th", "td", "ul", "ol", "li", "a", "img", "h1", "h2", "h3", "h4", "h5", "h6", "p", "b", "strong", "blockquote", "code" };

    /// <summary>
    /// Converts HTML to Markdown.
    /// </summary>
    /// <param name="html">The HTML input string.</param>
    /// <returns>Converted Markdown string.</returns>
    public static ConversionResult ConvertHtmlToMarkdown(string html)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(html)) return new ConversionResult { Markdown = string.Empty, Errors = errors };

        try
        {
            return new ConversionResult { Markdown = ParseHtml(html, errors), Errors = errors };
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
            return new ConversionResult { Markdown = string.Empty, Errors = errors };
        }
    }


    /// <summary>
    /// Parses HTML content into Markdown.
    /// </summary>
    private static string ParseHtml(string html, List<string> errors)
    {
        // First, normalize the input HTML to remove spaces and line breaks
        string normalizedHtml = NormalizeHtml(html);

        StringBuilder markdown = new StringBuilder();
        int position = 0;
        bool inTableHeader = false;
        int currentColumnCount = 0; // Temporary variable to track number of columns in the current table
        int blockquoteDepth = 0; // Track blockquote nesting level
        Stack<string> listTypeStack = new Stack<string>(); // Stack to track parent list types
        bool firstListItem = true; // Flag to track the first <li> in each list
        int listIndentLevel = 0;  // Track indentation level for nested lists
        bool ignoreContent = false; // Flag to track whether content should be ignored
        bool headerProcessed = false;  // To track if table header has been processed

        while (position < normalizedHtml.Length)
        {
            if (normalizedHtml[position] == '<')
            {
                int endTag = normalizedHtml.IndexOf('>', position);
                if (endTag == -1)
                {
                    errors.Add("Malformed HTML tag detected.");
                    break;
                }

                string tag = normalizedHtml.Substring(position + 1, endTag - position - 1).Trim();
                bool isSelfClosing = tag.EndsWith("/");  // Check if it's a self-closing tag
                string tagName = tag.Split(' ')[0].ToLower();

                bool isClosingTag = false;

                // Handle self-closing tag detection
                if (isSelfClosing)
                {
                    tagName = tagName.TrimEnd('/');
                }
                else
                {
                    isClosingTag = tagName.StartsWith("/");
                    if (isClosingTag)
                    {
                        tagName = tagName.Substring(1);
                    }
                }

                // Check if we need to start ignoring content for certain tags
                if (!isClosingTag && _ignoredTags.Contains(tagName))
                {
                    ignoreContent = true;
                }

                // Check if we're closing an ignored tag and stop ignoring content
                if (isClosingTag && _ignoredTags.Contains(tagName))
                {
                    ignoreContent = false;
                    position = endTag + 1; // Move past the closing tag
                    continue; // Skip processing for the ignored tag
                }

                // If content is currently being ignored, skip further processing
                if (ignoreContent)
                {
                    position = endTag + 1; // Move past the current tag and continue parsing
                    continue;
                }

                if (!_allowedTags.Contains(tagName) && !isSelfClosing && !isClosingTag)
                {
                    errors.Add($"Unrecognized tag: {tagName}");
                    position = endTag + 1;
                    continue;
                }

                switch (tagName)
                {
                    case "h1":
                    case "h2":
                    case "h3":
                    case "h4":
                    case "h5":
                    case "h6":
                        if (!isClosingTag)
                        {
                            int level = int.Parse(tagName.Substring(1));
                            markdown.Append(new string('#', level) + " ");
                        }
                        if (isClosingTag)
                        {
                            markdown.AppendLine().AppendLine(); // Ensure line break after heading
                        }
                        break;

                    case "p":
                        if (isClosingTag)
                        {
                            markdown.AppendLine().AppendLine(); // Paragraph should end with a double line break
                        }
                        break;

                    case "ul":
                    case "ol":
                        if (!isClosingTag)
                        {
                            listIndentLevel++;
                            listTypeStack.Push(tagName); // Push the current list type to the stack
                            firstListItem = true; // Reset the flag for the new list
                        }
                        else
                        {
                            listIndentLevel--;
                            listTypeStack.Pop(); // Pop the stack when closing the list
                            markdown.AppendLine();
                        }
                        break;

                    case "li":
                        if (!isClosingTag)
                        {
                            // If it's the first list item, insert a line break before the item
                            if (firstListItem)
                            {
                                markdown.AppendLine(); // Add a line break before the first list item
                                firstListItem = false; // After the first list item, set flag to false
                            }
                            // Get the current list type from the stack
                            string parentListType = listTypeStack.Count > 0 ? listTypeStack.Peek() : "ul";

                            // Append the appropriate list marker (* for ul, 1. for ol)
                            if (listIndentLevel > 1) //if indent level is more than one add a space before syntax (* or 1)
                            {
                                markdown.Append(new string('~', listIndentLevel) + (parentListType == "ul" ? " * " : " 1. "));
                            }
                            else
                                markdown.Append(new string('~', listIndentLevel) + (parentListType == "ul" ? "* " : "1. "));
                        }
                        if (isClosingTag)
                        {
                            markdown.AppendLine(); // Ensure line break after each list item
                        }
                        break;

                    case "blockquote":
                        if (!isClosingTag)
                        {
                            blockquoteDepth++; // Increase depth on opening blockquote
                            markdown.AppendLine().Append(new string('>', blockquoteDepth) + " ");
                        }
                        if (isClosingTag)
                        {
                            markdown.AppendLine(); // Line break after closing blockquote
                            if (blockquoteDepth > 1)
                            {
                                markdown.Append("> ");
                            }
                            blockquoteDepth--; // Decrease depth on closing blockquote
                        }
                        break;

                    case "code":
                        markdown.Append(isClosingTag ? "`" : "`");
                        break;

                    case "a":
                        if (!isClosingTag)
                        {
                            string href = ExtractAttribute(tag, "href");
                            // Extract anchor text for the closing tag
                            int startContent = normalizedHtml.IndexOf('>', position) + 1;
                            int endContent = normalizedHtml.IndexOf('<', startContent);
                            string anchorText = normalizedHtml.Substring(startContent, endContent - startContent).Trim();

                            markdown.Append($"[{anchorText}]({href})");
                            position = endContent; // Move position to the end of anchor tag content
                        }
                        break;
                    case "b":
                    case "strong":
                        markdown.Append(isClosingTag ? "**" : "**");
                        break;

                    case "i":
                    case "em":
                        markdown.Append(isClosingTag ? "_" : "_");
                        break;
                    case "img":
                        string src = ExtractAttribute(tag, "src");
                        string alt = ExtractAttribute(tag, "alt");
                        markdown.Append($"![{alt}]({src})");
                        break;
                    case "br":
                        markdown.AppendLine(); // Line break for <br> tag
                        break;

                    case "hr":
                        markdown.AppendLine().AppendLine("---"); // Horizontal rule in Markdown
                        break;

                    case "table":
                    case "tr":
                    case "th":
                    case "td":
                    case "thead":
                    case "tbody":
                        ProcessTableTag(normalizedHtml, tagName, isClosingTag, ref position, ref markdown, ref currentColumnCount, ref headerProcessed, ref inTableHeader);
                        break;

                    case "form":
                        if (isClosingTag)
                        {
                            markdown.AppendLine("> **Form End**").AppendLine(); // Form handling placeholder
                        }
                        break;

                    case "input":
                        string inputType = ExtractAttribute(tag, "type");
                        markdown.AppendLine($"> Input (Type: {inputType})"); // Input handling placeholder
                        break;

                    case "button":
                        if (!isClosingTag)
                        {
                            string buttonText = ExtractInnerContent(normalizedHtml, ref position);
                            markdown.AppendLine($"> **Button**: {buttonText}");
                        }
                        break;

                    case "span":
                    case "label":
                        // Just capture content within these tags, no markdown representation needed
                        break;

                    case "iframe":
                        string iframeSrc = ExtractAttribute(tag, "src");
                        markdown.AppendLine($"> **Embedded Content**: [iframe link]({iframeSrc})");
                        break;

                    default:
                        errors.Add($"Unrecognized tag: {tagName}");
                        break;
                }

                // Move past the tag               
                position = (position < (endTag + 1)) ? endTag + 1 : position;
            }
            else
            {
                // If we are not inside ignored content, append normal content
                if (!ignoreContent)
                {
                    markdown.Append(normalizedHtml[position]);
                }
                position++;
            }
        }

        // After processing, remove leading spaces from each line
        string[] lines = markdown.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        bool endsWithLineBreak = markdown.ToString().EndsWith("\r\n") || markdown.ToString().EndsWith("\n");
        if (lines.Length == 1)
        {
            return markdown.ToString();
        }
        StringBuilder result = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.TrimStart().StartsWith('~'))
            {
                // Replace only the leading instances of ~ with spaces
                string modifiedLine = Regex.Replace(line.TrimStart(), @"^~+", match => new string(' ', match.Length));
                result.AppendLine(modifiedLine); // Remove leading spaces but keep line breaks
            }
            else
            {
                result.AppendLine(line.TrimStart()); // Remove leading spaces but keep line breaks
            }
        }

        string finalString = result.ToString();
        if (endsWithLineBreak)
        {
            // Find the position of the last occurrence of "\r\n"
            int lastIndex = finalString.LastIndexOf("\r\n");

            // Remove the last occurrence of "\r\n"
            if (lastIndex >= 0)
            {
                finalString = finalString.Remove(lastIndex, 2);
            }
        }
        return finalString;
    }

    private static void ProcessTableTag(string normalizedHtml, string tagName, bool isClosingTag, ref int position, ref StringBuilder markdown, ref int currentColumnCount, ref bool headerProcessed, ref bool inTableHeader)
    {
        switch (tagName)
        {
            case "table":
                if (!isClosingTag)
                {
                    currentColumnCount = 0; // Reset the column count at the start of a new table
                    headerProcessed = false;
                    markdown.AppendLine(); // Ensure a line break before the new table
                }
                if (isClosingTag)
                {
                    markdown.AppendLine(); // Ensure line break after table
                }
                break;

            case "tr":
                if (isClosingTag)
                {
                    // If the row had <th> tags but no <thead>, add the separator after processing the row
                    if (!headerProcessed && currentColumnCount > 0)
                    {
                        markdown.Append("|").AppendLine();
                        markdown.Append("|");
                        for (int i = 0; i < currentColumnCount; i++)
                        {
                            markdown.Append(" --- |");
                        }
                        markdown.AppendLine(); // End the separator line
                        headerProcessed = true;  // Mark header as processed
                    }
                    else
                        markdown.Append("|").AppendLine(); // Ensure line break after each table row
                }
                break;

            case "th":
                if (!isClosingTag)
                {
                    if (inTableHeader || !headerProcessed)
                    {
                        currentColumnCount++;
                    }
                    markdown.Append("| ");
                }
                break;
            case "td":
                if (!isClosingTag)
                {
                    markdown.Append("| ");
                }
                break;
            case "thead":
                if (!isClosingTag)
                {
                    inTableHeader = true; // Mark that we're inside the table header section
                }
                else if (isClosingTag && inTableHeader)
                {
                    if (!headerProcessed && currentColumnCount > 0)
                    {
                        // Generate the separator line dynamically based on the number of columns in the current table
                        markdown.Append("|");
                        for (int i = 0; i < currentColumnCount; i++)
                        {
                            markdown.Append(" --- |");
                        }
                        markdown.AppendLine(); // End the separator line
                        headerProcessed = true;  // Mark header as processed
                    }
                    inTableHeader = false; // Reset the flag after processing the header
                }
                break;
        }
    }


    /// <summary>
    /// Method to extract attributes from a tag
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    private static string ExtractAttribute(string tag, string attribute)
    {
        // Find the start index of the attribute
        int startIndex = tag.IndexOf(attribute + "=", StringComparison.Ordinal);
        if (startIndex == -1)
            return string.Empty;

        // Move the start index to the position after the `=` symbol
        startIndex += attribute.Length + 1;

        // Determine the quote character (if any) and find the end index
        char quoteChar = tag[startIndex];
        if (quoteChar == '"' || quoteChar == '\'')
        {
            startIndex++; // Skip the quote character
            int endIndex = tag.IndexOf(quoteChar, startIndex);
            if (endIndex == -1)
                return string.Empty; // Malformed HTML, no closing quote
            return tag.Substring(startIndex, endIndex - startIndex);
        }
        else
        {
            // Handle cases without quotes (e.g., src=proto:path)
            int endIndex = tag.IndexOf(' ', startIndex);
            if (endIndex == -1) endIndex = tag.Length; // Attribute goes till end
            return tag.Substring(startIndex, endIndex - startIndex);
        }
    }



    /// <summary>
    /// Method to extract inner content of a tag, needed for elements like 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static string ExtractInnerContent(string html, ref int position)
    {
        int startContent = html.IndexOf('>', position) + 1;
        int endContent = html.IndexOf('<', startContent);
        if (endContent == -1)
            return string.Empty;

        position = endContent;
        return html.Substring(startContent, endContent - startContent).Trim();
    }


    /// <summary>
    /// Normalize the html except spaces between tags
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private static string NormalizeHtml(string html)
    {
        // Use a StringBuilder to build the normalized HTML string
        var sb = new StringBuilder(html.Length);

        // Iterate through the input HTML string
        for (int i = 0; i < html.Length; i++)
        {
            char c = html[i];

            // Replace tabs with a single space
            if (c == '\t')
            {
                sb.Append(' ');
            }
            // Append the character if it's not a newline or carriage return
            else if (c != '\n' && c != '\r')
            {
                sb.Append(c);
            }
        }

        // Apply Regex.Replace to remove extra spaces after building the string
        string normalizedHtml = sb.ToString();
        return Regex.Replace(normalizedHtml, @"\s{2,}", " ").Trim();
    }
}

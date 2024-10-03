using System.Text;
using System.Text.RegularExpressions;

namespace HtmlToMarkdownService;

/// <summary>
/// Service class to handle all conversion of html to marldown
/// </summary>
public class ConversionService
{
    private List<string> _errorLogs = new List<string>();
    private HashSet<string> _ignoredTags;

    /// <inheritdoc/>
    public ConversionService()
    {
        _ignoredTags = new HashSet<string> { "head", "title", "style", "script" }; // Tags to ignore
    }

    /// <summary>
    /// Main method to convert HTML to Markdown
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public string ConvertHtmlToMarkdown(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            LogError("Input HTML string is null or empty.");
            return string.Empty;
        }

        StringBuilder markdown = new StringBuilder();

        try
        {
            // Remove unnecessary whitespace
            html = Regex.Replace(html, @"\s+", " ").Trim();

            // Parse and convert
            markdown.Append(ParseHtml(html));
        }
        catch (Exception ex)
        {
            LogError($"Exception occurred: {ex.Message}");
        }

        return markdown.ToString();
    }

    /// <summary>
    /// List the errors during conversion
    /// </summary>
    /// <returns></returns>
    public List<string> GetErrorLogs()
    {
        return _errorLogs;
    }

    /// <summary>
    /// Method to handle parsing of HTML elements
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private string ParseHtml(string html)
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

        while (position < normalizedHtml.Length)
        {
            if (normalizedHtml[position] == '<')
            {
                int endTag = normalizedHtml.IndexOf('>', position);
                if (endTag == -1)
                {
                    LogError("Malformed HTML tag detected.");
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
                        if (!isClosingTag)
                        {
                            markdown.Append("`");
                        }
                        else
                        {
                            markdown.Append("`");
                        }
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
                        if (!isClosingTag)
                        {
                            markdown.Append("**");
                        }
                        else
                        {
                            markdown.Append("**");
                        }
                        break;

                    case "i":
                    case "em":
                        if (!isClosingTag)
                        {
                            markdown.Append("_");
                        }
                        else
                        {
                            markdown.Append("_");
                        }
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
                        if (!isClosingTag)
                        {
                            currentColumnCount = 0; // Reset the column count at the start of a new table
                        }
                        if (isClosingTag)
                        {
                            markdown.AppendLine(); // Ensure line break after table
                        }
                        break;

                    case "tr":
                        if (isClosingTag)
                        {
                            markdown.AppendLine(); // Ensure line break after each table row
                        }
                        break;


                    case "th":
                        if (!isClosingTag)
                        {
                            if (inTableHeader)
                            {
                                markdown.Append("| ");
                                currentColumnCount++;
                            }
                        }
                        else
                        {
                            inTableHeader = true;
                        }
                        break;
                    case "td":
                        if (!isClosingTag)
                        {
                            markdown.Append("| ");
                        }
                        break;
                    case "thead":
                        if (isClosingTag && inTableHeader)
                        {
                            // Generate the separator line dynamically based on the number of columns in the current table
                            markdown.Append("|");
                            for (int i = 0; i < currentColumnCount; i++)
                            {
                                markdown.Append(" --- |");
                            }
                            markdown.AppendLine(); // End the separator line

                            inTableHeader = false; // Reset the flag after processing the header
                        }
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
                        LogError($"Unrecognized tag: {tagName}");
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
        //checks if generated string has linebreaks at end
        bool endsWithLineBreak = markdown.ToString().EndsWith("\r\n") || markdown.ToString().EndsWith("\n");
        if (lines.Length == 1)
        {
            return markdown.ToString();
        }
        StringBuilder result = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.StartsWith('~'))
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

    /// <summary>
    /// Normalize the html except spaces between tags
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private string NormalizeHtml(string html)
    {
        // Step 1: Remove line breaks and excess spacing from HTML input
        string normalizedHtml = html
            .Replace("\n", "")       // Remove newlines
            .Replace("\r", "")       // Remove carriage returns
            .Replace("\t", " ");      // Replace tabs with single space
                                      //.Replace("> <", "><");   // Remove spaces between tags

        // Step 2: Replace multiple spaces with a single space
        normalizedHtml = Regex.Replace(normalizedHtml, @"\s{2,}", " ");

        return normalizedHtml.Trim(); // Return the trimmed and normalized HTML
    }

    /// <summary>
    /// Method to extract attributes from a tag
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    private string ExtractAttribute(string tag, string attribute)
    {
        string pattern = $@"{attribute}\s*=\s*[""']([^""']+)[""']";
        Match match = Regex.Match(tag, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    /// <summary>
    /// Method to extract inner content of a tag, needed for elements like 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private string ExtractInnerContent(string html, ref int position)
    {
        int startContent = html.IndexOf('>', position) + 1;
        int endContent = html.IndexOf('<', startContent);
        position = endContent;  // Move position to the end of the inner content
        return html.Substring(startContent, endContent - startContent).Trim();
    }

    /// <summary>
    /// Method to log errors
    /// </summary>
    /// <param name="message"></param>
    private void LogError(string message)
    {
        _errorLogs.Add(message);
    }
}

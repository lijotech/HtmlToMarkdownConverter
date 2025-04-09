namespace HtmlToMarkdown.Service
{
    /// <summary>
    /// Result of the Conversion
    /// </summary>
    public class ConversionResult
    {
        /// <summary>
        /// Markdown output string
        /// </summary>
        public string Markdown { get; set; } = string.Empty;
        /// <summary>
        /// Retrieves any errors encountered during conversion.
        /// </summary>
        public List<string> Errors { get; set; } = new();
    }
}

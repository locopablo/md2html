using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace md2html
{
    public class PreProcessor
    {
        private Configuration _Configuration;

        private PreProcessor(Configuration configuration)
        {
            _Configuration = configuration;
        }

        public static string ReadAndProcess(string fileName, Configuration configuration)
        {
            if (Object.ReferenceEquals(null, fileName) || fileName.Trim().Length == 0)
                throw new ArgumentNullException("fileName");
            if (Object.ReferenceEquals(null, configuration))
                throw new ArgumentNullException("configuration");

            PreProcessor processor = new PreProcessor(configuration);
            IEnumerable<MarkdownSourceLineEntry> lines = MarkdownSourceReader.Read(fileName);
            IEnumerable<MarkdownSourceLineEntry> processedLines = processor.Process(lines);

            return string.Join(Environment.NewLine,
                (from line in processedLines
                 select line.Line).ToArray());
        }

        private IEnumerable<MarkdownSourceLineEntry> Process(IEnumerable<MarkdownSourceLineEntry> entries)
        {
            foreach (MarkdownSourceLineEntry entry in entries)
            {
                if (entry.Line.StartsWith("@@ "))
                {
                    foreach (MarkdownSourceLineEntry newEntry in ProcessDirective(entry))
                        yield return newEntry;
                }
                else
                    yield return entry;
            }
        }

        private string[] SplitArguments(string argumentsString)
        {
            List<string> result = new List<string>();

            int index = 0;
            while (index < argumentsString.Length)
            {
                if (char.IsWhiteSpace(argumentsString[index]))
                    index++;
                else
                {
                    if (argumentsString[index] == '"')
                    {
                        index++;
                        StringBuilder quotedArg = new StringBuilder();
                        while (index < argumentsString.Length)
                        {
                            if (argumentsString[index] == '"')
                            {
                                index++;
                                break;
                            }
                            else if (argumentsString[index] == '\\')
                            {
                                index++;
                                if (index < argumentsString.Length)
                                {
                                    quotedArg.Append(argumentsString[index]);
                                    index++;
                                }
                            }
                            else
                            {
                                quotedArg.Append(argumentsString[index]);
                                index++;
                            }
                        }
                        result.Add(quotedArg.ToString());
                    }
                    else
                    {
                        int start = index;
                        while (index < argumentsString.Length && !char.IsWhiteSpace(argumentsString[index]))
                            index++;
                        result.Add(argumentsString.Substring(start, index - start));
                    }
                }
            }
            return result.ToArray();
        }

        private IEnumerable<MarkdownSourceLineEntry> ProcessDirective(MarkdownSourceLineEntry entry)
        {
            string directiveLine = entry.Line.Substring(3);
            if (directiveLine.TrimStart() != directiveLine)
                throw new ErrorMessageException(String.Format(md2html.Properties.Resources.DirectiveNameMustFollowSingleSpaceErrorMessage, entry.FileName, entry.LineNumber, entry.Line));

            string directiveName;
            string[] directiveArguments;

            int indexOfSpace = directiveLine.IndexOf(' ');
            if (indexOfSpace == 0)
            {
                directiveName = directiveLine;
                directiveArguments = new string[0];
            }
            else
            {
                directiveName = directiveLine.Substring(0, indexOfSpace);
                directiveArguments = SplitArguments(directiveLine.Substring(indexOfSpace + 1));
            }

            switch (directiveName)
            {
                case "title":
                    return ProcessTitleDirective(entry, directiveArguments);
                    
                case "ignore":
                    return ProcessIgnoreDirective(entry, directiveArguments);

                default:
                    throw new ErrorMessageException(String.Format(Properties.Resources.UnknownDirectiveErrorMessage, directiveName, entry.FileName, entry.LineNumber, entry.Line));
            }
        }

        private IEnumerable<MarkdownSourceLineEntry> ProcessTitleDirective(MarkdownSourceLineEntry entry, string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ErrorMessageException(
                    String.Format(Properties.Resources.TitleDirectiveNeedsExactlyOneArgumentErrorMessage, arguments.Length,
                        entry.FileName, entry.LineNumber, entry.Line));
            }

            if (_Configuration.Title != string.Empty)
            {
                if (!_Configuration.IgnoreWarnings.Contains("W001"))
                    Console.Error.WriteLine(String.Format(Properties.Resources.MultipleTitlesSpecifiedWarningMessage, arguments[0]));
            }
            _Configuration.Title = arguments[0];

            yield break;
        }

        private IEnumerable<MarkdownSourceLineEntry> ProcessIgnoreDirective(MarkdownSourceLineEntry entry, string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ErrorMessageException(
                    String.Format(Properties.Resources.IgnoreDirectiveNeedsExactlyOneArgumentErrorMessage, arguments.Length,
                        entry.FileName, entry.LineNumber, entry.Line));
            }

            string warningCode = arguments[0].ToUpper();
            switch (warningCode)
            {
                case "W001":
                    _Configuration.IgnoreWarnings.Add(warningCode);
                    break;

                default:
                    throw new ErrorMessageException(
                        String.Format(Properties.Resources.IgnoreDirectiveUnknownWarningCodeErrorMessage,
                            entry.FileName, entry.LineNumber, entry.Line));
            }

            yield break;
        }
    }
}

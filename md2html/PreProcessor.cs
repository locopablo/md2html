using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownSharp;
using System.IO;

namespace md2html
{
    public class PreProcessor
    {
        private PreProcessor()
        {
        }

        public static string ReadAndProcess(string fileName)
        {
            if (Object.ReferenceEquals(null, fileName) || fileName.Trim().Length == 0)
                throw new ArgumentNullException("fileName");

            PreProcessor processor = new PreProcessor();
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
                directiveArguments = new string[0];
            }

            switch (directiveName)
            {
                case "title":
                    return ProcessTitleDirective(entry, directiveArguments);

                default:
                    throw new ErrorMessageException(String.Format(Properties.Resources.UnknownDirectiveErrorMessage, directiveName, entry.FileName, entry.LineNumber, entry.Line));
            }
        }

        private IEnumerable<MarkdownSourceLineEntry> ProcessTitleDirective(MarkdownSourceLineEntry entry, string[] arguments)
        {
            yield break;
        }
    }
}

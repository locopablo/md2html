using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace md2html
{
    public class MarkdownSourceLineEntry
    {
        public MarkdownSourceLineEntry(string fileName, int lineNumber, string line)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            Line = line;
        }

        public string FileName
        {
            get;
            private set;
        }

        public int LineNumber
        {
            get;
            private set;
        }

        public string Line
        {
            get;
            private set;
        }
    }
}

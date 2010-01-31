using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace md2html
{
    public static class MarkdownSourceReader
    {
        public static IEnumerable<MarkdownSourceLineEntry> Read(string fileName)
        {
            if (Object.ReferenceEquals(null, fileName) || fileName.Trim().Length == 0)
                throw new ArgumentNullException("fileName");

            using (StreamReader reader = new StreamReader(fileName))
            {
                int lineNumber = 0;
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;

                    yield return new MarkdownSourceLineEntry(fileName, lineNumber, line);
                }
            }
        }
    }
}

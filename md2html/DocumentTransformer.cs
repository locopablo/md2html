﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownSharp;
using System.IO;

namespace md2html
{
    public class DocumentTransformer
    {
        private static string ReadInputFile(Configuration configuration)
        {
            string input;
            try
            {
                input = PreProcessor.ReadAndProcess(configuration.InputFileName, configuration);
            }
            catch (FileNotFoundException ex)
            {
                throw new ErrorMessageException(String.Format(md2html.Properties.Resources.InputFileNotFoundErrorMessage, configuration.InputFileName, ex.Message), ex);
            }
            return input;
        }

        private static void WriteOutputFile(Configuration configuration, string output)
        {
            using (StreamWriter writer = new StreamWriter(configuration.OutputFileName, false, Encoding.UTF8))
            {
                writer.Write(output);
            }
        }

        public static void Transform(Configuration configuration)
        {
            if (Object.ReferenceEquals(null, configuration))
                throw new ArgumentNullException("configuration");

            Markdown md = new Markdown();

            string input = ReadInputFile(configuration);
            string html = md.Transform(input);
            string finalHtml = PostProcessor.Process(html, configuration);
            WriteOutputFile(configuration, finalHtml);
        }
    }
}

﻿using MarkdownSharp;
using System;
using System.Reflection;
using md2html.Properties;
using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;

namespace md2html
{
    public class Program
    {
        private static void Welcome(Configuration configuration)
        {
            if (configuration.Verbose)
                Console.Out.WriteLine(String.Format(md2html.Properties.Resources.VersionMessageWithMarkdown, Assembly.GetExecutingAssembly().GetName().Version, new Markdown().Version));
            else
                Console.Out.WriteLine(String.Format(md2html.Properties.Resources.VersionMessageWithoutMarkdown, Assembly.GetExecutingAssembly().GetName().Version));
        }

        private static void ShowHelp(Configuration configuration)
        {
            Welcome(configuration);
            Console.Out.WriteLine(Resources.HelpText);
        }

        private static void TransformFile(Configuration configuration)
        {
            Markdown md = new Markdown();

            string input;
            using (StreamReader reader = new StreamReader(configuration.InputFileName))
            {
                input = reader.ReadToEnd();
            }
            string output = md.Transform(input);
            using (StreamWriter writer = new StreamWriter(configuration.OutputFileName, false, Encoding.UTF8))
            {
                writer.Write(output);
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                Configuration configuration = new Configuration();
                CommandLineArguments.Configure(configuration, args);

                if (configuration.Verbose && configuration.Action != Actions.ShowHelp)
                    Welcome(configuration);

                switch (configuration.Action)
                {
                    case Actions.ShowHelp:
                        ShowHelp(configuration);
                        Environment.ExitCode = 0;
                        return;

                    case Actions.Unknown:
                        throw new Exception(Properties.Resources.UnknownActionErrorMessage);

                    case Actions.TransformFile:
                        TransformFile(configuration);
                        Environment.ExitCode = 0;
                        return;

                    default:
                        throw new Exception(String.Format(Properties.Resources.UnableToProcessActionErrorMessage, configuration.Action));
                }
            }
            catch (ErrorMessageException ex)
            {
                Console.Error.WriteLine(string.Format(md2html.Properties.Resources.ErrorMessage, ex.Message));
                Environment.ExitCode = 1;
                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format(md2html.Properties.Resources.BugMessage, ex.Message, ex.StackTrace));
                Environment.ExitCode = 999;
                return;
            }
        }
    }
}

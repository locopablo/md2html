using System;
using System.Text.RegularExpressions;

namespace md2html
{
    public static class CommandLineArguments
    {
        public static void Configure(Configuration configuration, string[] args)
        {
            if (Object.ReferenceEquals(null, configuration))
                throw new ArgumentNullException("configuration");
            if (Object.ReferenceEquals(null, args))
                throw new ArgumentNullException("args");

            Regex reOption = new Regex(@"^(?<option>(/|--?)(\w+|\?))([:=](?<arg>.+))?$", RegexOptions.IgnoreCase);
            foreach (string arg in args)
            {
                Match maOption = reOption.Match(arg);
                if (maOption.Success)
                {
                    switch (maOption.Groups["option"].Value)
                    {
                        case "/?":
                        case "/h":
                        case "/help":
                        case "-?":
                        case "-h":
                        case "--help":
                            configuration.Action = Actions.ShowHelp;
                            break;

                        case "/v":
                        case "/verbose":
                        case "-v":
                        case "--verbose":
                            configuration.Verbose = true;
                            break;

                        case "/t":
                        case "/title":
                        case "-t":
                        case "--title":
                            if (maOption.Groups["arg"].Success)
                                configuration.Title = maOption.Groups["arg"].Value;
                            else
                                throw new ErrorMessageException("--title option needs to specify a title");
                            break;

                        default:
                            throw new ErrorMessageException(String.Format(Properties.Resources.UnknownOptionErrorMessage, arg));
                    }
                }
                else
                {
                    if (configuration.InputFileName == string.Empty)
                        configuration.InputFileName = arg;
                    else if (configuration.OutputFileName == string.Empty)
                        configuration.OutputFileName = arg;
                    else
                        throw new ErrorMessageException(String.Format(Properties.Resources.TooManyFilesErrorMessage, arg));
                }

            }

            configuration.DeduceAction();
        }
    }
}

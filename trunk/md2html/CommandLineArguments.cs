using System;

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

            if (args.Length == 2)
            {
                configuration.InputFileName = args[0];
                configuration.OutputFileName = args[1];
            }
        }
    }
}

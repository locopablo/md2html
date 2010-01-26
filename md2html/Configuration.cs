using System;

namespace md2html
{
    public class Configuration
    {
        public Configuration()
        {
            InputFileName = string.Empty;
            OutputFileName = string.Empty;
        }

        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
    }
}

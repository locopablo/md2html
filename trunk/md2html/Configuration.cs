using System;
using System.IO;
using System.Diagnostics;

namespace md2html
{
    public class Configuration
    {
        public Configuration()
        {
            InputFileName = string.Empty;
            OutputFileName = string.Empty;
            Title = string.Empty;
            Action = Actions.Unknown;
            Verbose = false;
        }

        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public string Title { get; set; }
        public Actions Action { get; set; }
        public bool Verbose { get; set; }

        public void DeduceAction()
        {
            if (Action != Actions.Unknown)
                return;

            if (InputFileName != string.Empty)
            {
                if (OutputFileName == string.Empty)
                    OutputFileName = Path.ChangeExtension(InputFileName, ".html");

                if (Title == string.Empty)
                    Title = Path.GetFileName(InputFileName);
            }

            if (InputFileName != string.Empty && OutputFileName != string.Empty)
                Action = Actions.TransformFile;

            if (InputFileName == string.Empty)
            {
                Debug.Assert(OutputFileName == string.Empty);
                throw new ErrorMessageException(Properties.Resources.NothingToDoMessage);
            }
        }
    }
}

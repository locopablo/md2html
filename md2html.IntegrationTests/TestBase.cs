using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace md2html.IntegrationTests
{
    public class TestBase
    {
        protected string InputFileName { get; private set; }
        protected string OutputFileName { get; private set; }

        [SetUp]
        public void SetUp()
        {
            InputFileName = Path.GetTempFileName();
            OutputFileName = Path.GetTempFileName();

            if (File.Exists(InputFileName))
                File.Delete(InputFileName);

            using (StreamWriter writer = new StreamWriter(OutputFileName))
            {
                // make sure the file is not empty, or predictable
                writer.Write(new Guid().ToString());
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (string fileName in new[] { InputFileName, OutputFileName })
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                        // Do nothing here
                    }
                }
            }
        }
    }
}

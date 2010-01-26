using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace md2html.Tests
{
    [TestFixture]
    public class ConsoleOutputTests
    {
        private struct Result
        {
            public string Output;
            public string Error;
            public int ExitCode;
        }

        private Result GetOutput(params string[] args)
        {
            using (StringWriter output = new StringWriter())
            using (StringWriter error = new StringWriter())
            {
                Console.SetOut(output);
                Console.SetError(error);
                Environment.ExitCode = 0;

                Program.Main(args);

                Result result = new Result();
                result.Output = output.ToString();
                result.Error = error.ToString();
                result.ExitCode = Environment.ExitCode;
                return result;
            }
        }

        [Test]
        public void NoInput_NoOutput()
        {
            Result result = GetOutput();
            Assert.That(result.Output, Is.EqualTo(string.Empty));
            Assert.That(result.Error, Is.EqualTo(string.Empty));
            Assert.That(result.ExitCode, Is.EqualTo(0));
        }
    }
}

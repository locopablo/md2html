using System;
using NUnit.Framework;
using System.IO;
using md2html.IntegrationTests;

namespace md2html.IntegrationTests
{
    [TestFixture]
    public class ConsoleOutputTests : TestBase
    {
        private struct Result
        {
            public string Output;
            public string Error;
            public int ExitCode;
        }

        private Result GetOutput(params string[] args)
        {
            TextWriter oldOutput = Console.Out;
            TextWriter oldError = Console.Error;
            try
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
            finally
            {
                Console.SetOut(oldOutput);
                Console.SetError(oldError);
            }
        }

        private void VerifyNoErrors(Result result)
        {
            Assert.That(result.Error, Is.EqualTo(string.Empty));
            Assert.That(result.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void TooManyFiles_ProducesErrorMessageToThatEffect()
        {
            Result result = GetOutput("a", "b", "c");
            Assert.That(result.Error.Contains("too many filenames"), Is.True);
            Assert.That(result.ExitCode, Is.EqualTo(1));
        }

        [Test]
        public void UnknownOption_ProducesErrorMessageToThatEffect()
        {
            Result result = GetOutput("/xxxx");
            Assert.That(result.Error.Contains("unknown option"), Is.True);
            Assert.That(result.ExitCode, Is.EqualTo(1));
        }

        [Test]
        public void NoInput_NoOutput()
        {
            Result result = GetOutput();
            Assert.That(result.Error.Contains("no file(s) specified"), Is.True);
            Assert.That(result.ExitCode, Is.EqualTo(1));
        }

        [TestCase("-h")]
        [TestCase("--help")]
        [TestCase("-?")]
        [TestCase("/?")]
        [TestCase("/h")]
        [TestCase("/help")]
        public void HelpOption_ProducesHelpText(string option)
        {
            Result result = GetOutput(option);
            VerifyNoErrors(result);
            Assert.That(result.Output.Contains("usage:"), Is.True);
            Assert.That(result.Output.Contains("options:"), Is.True);

        }

        [TestCase("")]
        [TestCase("-v")]
        [TestCase("--verbose")]
        [TestCase("/v")]
        [TestCase("/verbose")]
        public void VerboseOption_EnablesVerbosity(string option)
        {
            Result result;
            if (option != "")
                result = GetOutput("-h", option);
            else
                result = GetOutput("-h");
            VerifyNoErrors(result);
            Assert.That(result.Output.Contains("usage:"), Is.True);
            Assert.That(result.Output.Contains("options:"), Is.True);
            if (option != "")
                Assert.That(result.Output.Contains("MarkdownSharp v"), Is.True);
            else
                Assert.That(result.Output.Contains("MarkdownSharp v"), Is.False);
        }

        [Test]
        public void Transform_WithoutVerbose_OutputsNothing()
        {
            File.WriteAllText(InputFileName, string.Empty);
            Result result = GetOutput(InputFileName, OutputFileName);
            Assert.That(result.Output, Is.EqualTo(string.Empty));
            Assert.That(result.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void Transform_WithVerbose_OutputsNothing()
        {
            File.WriteAllText(InputFileName, string.Empty);
            Result result = GetOutput("-v", InputFileName, OutputFileName);
            Assert.That(result.Output.Contains("md2html v"), Is.True);
            Assert.That(result.ExitCode, Is.EqualTo(0));
        }
    }
}

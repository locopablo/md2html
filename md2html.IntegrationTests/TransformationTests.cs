using System;
using NUnit.Framework;
using System.IO;

namespace md2html.IntegrationTests
{
    [TestFixture]
    public class TransformationTests : TestBase
    {
        private string Transform(string input)
        {
            using (StreamWriter writer = new StreamWriter(InputFileName))
            {
                writer.Write(input);
            }

            Program.Main(new[] { InputFileName, OutputFileName });

            using (StreamReader reader = new StreamReader(OutputFileName))
            {
                return reader.ReadToEnd();
            }
        }

        [Test]
        public void Transform_EmptyInput_ProducesEmptyOutput()
        {
            Assert.That(Transform(string.Empty), Is.EqualTo(string.Empty));
        }
    }
}

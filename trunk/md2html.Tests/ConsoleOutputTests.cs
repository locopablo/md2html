using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace md2html.Tests
{
    [TestFixture]
    public class ConsoleOutputTests
    {
        private string GetOutput(params string[] args)
        {
            Program.Main(args);
            return string.Empty;
        }

        [Test]
        public void NoInput_NoOutput()
        {
            string output = GetOutput();
            Assert.That(output, Is.EqualTo(string.Empty));
        }
    }
}

using System;
using NUnit.Framework;

namespace md2html.UnitTests
{
    [TestFixture]
    public class CommandLineArgumentsTests
    {
        [Test]
        public void Configure_Constructor_ProducesConfigurationWithNoInputAndOutputFileNames()
        {
            Configuration configuration = new Configuration();
            Assert.That(configuration.InputFileName, Is.EqualTo(string.Empty));
            Assert.That(configuration.OutputFileName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Configure_NullConfigurationArgument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                CommandLineArguments.Configure(null, new[] { "dummy" });
            });
        }

        [Test]
        public void Configure_NullArgsArray_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                CommandLineArguments.Configure(new Configuration(), null);
            });
        }

        [Test]
        public void Configure_TwoFileNames_SetAsInputAndOutputFileNames()
        {
            Configuration configuration = new Configuration();
            CommandLineArguments.Configure(configuration, new[] { "a", "b" });
            Assert.That(configuration.InputFileName, Is.EqualTo("a"));
            Assert.That(configuration.OutputFileName, Is.EqualTo("b"));
        }
    }
}

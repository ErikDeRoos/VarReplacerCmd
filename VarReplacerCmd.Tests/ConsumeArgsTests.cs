using Microsoft.VisualStudio.TestTools.UnitTesting;
using VarReplacerCmd.Args;

namespace VarReplacerCmd.Tests
{
    [TestClass]
    public class ConsumeArgsTests
    {
        [TestMethod]
        public void TriggerHelp()
        {
            var data = new[]
            {
                "-h"
            };

            var sut = new ConsumeArgs(data);

            Assert.IsTrue(sut.ShowHelp());
        }

        [TestMethod]
        public void ParseErrorOnEmpty()
        {
            var data = new[]
            {
                ""
            };

            var sut = new ConsumeArgs(data);

            Assert.IsTrue(sut.ParseError());
        }

        [TestMethod]
        public void ParseErrorIfNotAtLeastOneFile()
        {
            var data = new[]
            {
                "-f"
            };

            var sut = new ConsumeArgs(data);

            Assert.IsTrue(sut.ParseError());
        }

        [TestMethod]
        public void SingleFileSucceeds()
        {
            var data = new[]
            {
                "-f",
                "file"
            };

            var sut = new ConsumeArgs(data);

            Assert.IsFalse(sut.ParseError());
        }

        [TestMethod]
        public void SearchSubDirectories()
        {
            var data = new[]
            {
                "-f",
                "file",
                "-s"
            };

            var sut = new ConsumeArgs(data);
            var result = sut.GetFileSearchPattern();

            Assert.IsFalse(sut.ParseError());
            Assert.IsTrue(result.IncludeSubdirectories);
        }

        [TestMethod]
        public void SingleFileParses()
        {
            var data = new[]
            {
                "-f",
                "file"
            };

            var sut = new ConsumeArgs(data);
            var result = sut.GetFileSearchPattern();

            Assert.AreEqual(data[1], result.SearchPattern[0]);
        }

        [TestMethod]
        public void MultiFileParses()
        {
            var data = new[]
            {
                "-f",
                "file1",
                "file2"
            };

            var sut = new ConsumeArgs(data);
            var result = sut.GetFileSearchPattern();

            Assert.AreEqual(data[1], result.SearchPattern[0]);
            Assert.AreEqual(data[2], result.SearchPattern[1]);
        }

        [TestMethod]
        public void MultiArgumentParses()
        {
            var data = new[]
            {
                "-f",
                "file1",
                "file2",
                "-s"
            };

            var sut = new ConsumeArgs(data);
            var result = sut.GetFileSearchPattern();

            Assert.AreEqual(data[2], result.SearchPattern[1]);
            Assert.IsTrue(result.IncludeSubdirectories);
        }

    }
}

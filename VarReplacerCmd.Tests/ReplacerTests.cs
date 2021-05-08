using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using VarReplacerCmd.Replace;

namespace VarReplacerCmd.Tests
{
    [TestClass]
    public class ReplacerTests
    {
        [TestMethod]
        public void ReplaceBasicLine()
        {
            var data = new[]
            {
                "No idea",
                "But this $(line) needs replacing"
            };
            var fixedLine = data[1].Replace("$(line)", "data");

            var lookup = Mock.Of<IReferenceLookup>();
            Mock.Get(lookup)
                .Setup(x => x.Lookup("line")).Returns("data");

            var sut = new Replacer(lookup);
            var processed = sut.ReplaceContent(data).ToArray();

            Assert.AreEqual(data.Length, processed.Length);
            Assert.AreEqual(fixedLine, processed[1]);
        }

        [TestMethod]
        public void ReplaceMultiVarLine()
        {
            var data = new[]
            {
                "No idea",
                "But this $(line) needs replacing but also here $(line)."
            };
            var fixedLine = data[1].Replace("$(line)", "data");

            var lookup = Mock.Of<IReferenceLookup>();
            Mock.Get(lookup)
                .Setup(x => x.Lookup("line")).Returns("data");

            var sut = new Replacer(lookup);
            var processed = sut.ReplaceContent(data).ToArray();

            Assert.AreEqual(data.Length, processed.Length);
            Assert.AreEqual(fixedLine, processed[1]);
        }

        [TestMethod]
        public void ReplaceBasicLineComplexVar()
        {
            var data = new[]
            {
                "No idea",
                "But this complex var $(lI.n_e) needs replacing"
            };
            var fixedLine = data[1].Replace("$(lI.n_e)", "data");

            var lookup = Mock.Of<IReferenceLookup>();
            Mock.Get(lookup)
                .Setup(x => x.Lookup("lI.n_e")).Returns("data");

            var sut = new Replacer(lookup);
            var processed = sut.ReplaceContent(data).ToArray();

            Assert.AreEqual(data.Length, processed.Length);
            Assert.AreEqual(fixedLine, processed[1]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileQuery.Core.Filter;
using NUnit.Framework;

namespace FileQuery.Tests.Core
{
    [TestFixture]
    class FileExtensionFilterTests
    {
        [Test]
        public void WhenExtensionContainsDot_IsValid()
        {
            var filter = new TestFileExtensionFilter(".txt", ".ts", ".cs");
            Assert.AreEqual(".txt", filter.Extensions.ElementAt(0));
            Assert.AreEqual(".ts", filter.Extensions.ElementAt(1));
            Assert.AreEqual(".cs", filter.Extensions.ElementAt(2));
        }

        [Test]
        public void WhenExtensionNotContainsDot_IsValid()
        {
            var filter = new TestFileExtensionFilter("txt", "ts", "cs");
            Assert.AreEqual(".txt", filter.Extensions.ElementAt(0));
            Assert.AreEqual(".ts", filter.Extensions.ElementAt(1));
            Assert.AreEqual(".cs", filter.Extensions.ElementAt(2));
        }

        private class TestFileExtensionFilter : FileExtensionFilter
        {
            public TestFileExtensionFilter(params string[] extensions)
                : base(extensions)
            {
            }

            public IEnumerable<string> Extensions { get { return _extensions; } }
        }
    }
}

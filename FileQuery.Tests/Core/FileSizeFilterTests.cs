using System.Collections.Generic;
using FileQuery.Core;
using FileQuery.Core.Filter;
using NUnit.Framework;

namespace FileQuery.Tests.Core
{
    [TestFixture]
    class FileSizeFilterTests
    {
        const long kb = 1024L;
        const long mb = kb * 1024L;
        const long gb = mb * 1024L;

        [Test]
        public void WhenSizeIsNumberOnly_ThenIsKB()
        {
            var filter = new TestFileSizeFilter("1", "12", "123");
            Assert.AreEqual(1 * kb, filter.Sizes[0]);
            Assert.AreEqual(12 * kb, filter.Sizes[1]);
            Assert.AreEqual(123 * kb, filter.Sizes[2]);
            // Default for multiple values is IN
            Assert.AreEqual(FilterOperator.In, filter.FilterOperator);
        }

        [Test]
        public void WhenSizeIsDecimal_ThenIsConverted()
        {
            var filter = new TestFileSizeFilter("1.2", "1.2k", "1.2m", "1.2g");
            Assert.AreEqual((long)(1.2 * kb), filter.Sizes[0]);
            Assert.AreEqual((long)(1.2 * kb), filter.Sizes[1]);
            Assert.AreEqual((long)(1.2 * mb), filter.Sizes[2]);
            Assert.AreEqual((long)(1.2 * gb), filter.Sizes[3]);
        }

        [Test]
        public void WhenSizeIsInvalid_ThenError()
        {
            Assert.Catch<FileQueryException>(() => new TestFileSizeFilter("1b"));
            Assert.Catch<FileQueryException>(() => new TestFileSizeFilter("kb"));
            Assert.Catch<FileQueryException>(() => new TestFileSizeFilter("a"));
        }

        [Test]
        public void WhenSizeIsKB_ThenIsKB()
        {
            var filter = new TestFileSizeFilter("123K", "234k", "345kb");
            Assert.AreEqual(123 * kb, filter.Sizes[0]);
            Assert.AreEqual(234 * kb, filter.Sizes[1]);
            Assert.AreEqual(345 * kb, filter.Sizes[2]);
        }

        [Test]
        public void WhenSizeIsMB_ThenIsMB()
        {
            var filter = new TestFileSizeFilter("123M", "234m", "345mb");
            Assert.AreEqual(123 * mb, filter.Sizes[0]);
            Assert.AreEqual(234 * mb, filter.Sizes[1]);
            Assert.AreEqual(345 * mb, filter.Sizes[2]);
        }

        [Test]
        public void WhenSizeIsGB_ThenIsGB()
        {
            var filter = new TestFileSizeFilter("123G", "234g", "345gb");
            Assert.AreEqual(123 * gb, filter.Sizes[0]);
            Assert.AreEqual(234 * gb, filter.Sizes[1]);
            Assert.AreEqual(345 * gb, filter.Sizes[2]);
        }

        class TestFileSizeFilter : FileSizeFilter
        {
            public TestFileSizeFilter(string size)
                : base(size)
            { }
            public TestFileSizeFilter(params string[] sizes)
                : base(sizes)
            { }


            public new List<long> Sizes
            {
                get { return base.Sizes; }
            }
        }
    }
}

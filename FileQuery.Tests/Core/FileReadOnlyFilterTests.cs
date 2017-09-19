using FileQuery.Core;
using FileQuery.Core.Filter;
using NUnit.Framework;

namespace FileQuery.Tests.Core
{
    [TestFixture]
    class FileReadOnlyFilterTests
    {
        [Test]
        public void WhenCreateTrue_ThenValid()
        {
            var filter = new FileReadOnlyFilter("true");
            Assert.IsTrue(filter.IsReadOnly);
        }

        [Test]
        public void WhenCreateFalse_ThenValid()
        {
            var filter = new FileReadOnlyFilter("false");
            Assert.IsFalse(filter.IsReadOnly);
        }

        [Test]
        public void WhenInvalid_ThenError()
        {
            Assert.Catch<FileQueryException>(() => new FileReadOnlyFilter("foo"));
        }
    }
}

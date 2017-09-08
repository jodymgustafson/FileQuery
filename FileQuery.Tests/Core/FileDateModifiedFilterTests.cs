using System;
using System.Collections.Generic;
using FileQuery.Core;
using FileQuery.Core.Filter;
using NUnit.Framework;

namespace FileQuery.Tests.Core
{
    [TestFixture]
    class FileDateModifiedFilterTests
    {
        [Test]
        public void WhenValidDate_ThenValid()
        {
            var filter = new TestFileDateModifiedFilter("1/1/2017", FilterOperator.Equal);
            Assert.AreEqual(new DateTime(2017, 1, 1), filter.modifiedTimes[0]);
            Assert.AreEqual(FilterOperator.Equal, filter.FilterOperator);
        }

        [Test]
        public void WhenInvalidDate_ThenError()
        {
            Assert.Catch<FileQueryException>(() => new TestFileDateModifiedFilter("one/1/2017", FilterOperator.Equal));
        }

        [Test]
        public void WhenMultipleDates_ThenValid()
        {
            var filter = new TestFileDateModifiedFilter("1/1/2017", "10/11/2016");
            Assert.AreEqual(new DateTime(2017, 1, 1), filter.modifiedTimes[0]);
            Assert.AreEqual(new DateTime(2016, 10, 11), filter.modifiedTimes[1]);
            Assert.AreEqual(FilterOperator.In, filter.FilterOperator);
        }

        class TestFileDateModifiedFilter : FileDateModifiedFilter
        {
            public TestFileDateModifiedFilter(string sDate, FilterOperator op)
                : base(sDate, op)
            { }
            public TestFileDateModifiedFilter(params string[] dates)
                : base(dates)
            { }

            public new List<DateTime> modifiedTimes {  get { return base.modifiedTimes; } }
        }
    }
}

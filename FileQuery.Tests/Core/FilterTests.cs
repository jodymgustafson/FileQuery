using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileQuery.Core;
using FileQuery.Tests.Core;
using NUnit.Framework;

namespace FileQuery.Tests
{
    [TestFixture]
    public class FilterTests
    {
        private FileQueryProcessor proc;

        [SetUp]
        public void Setup()
        {
            proc = new FileQueryProcessor();
        }

        [Test]
        public void WhenInOperator_ThenGetFiles()
        {
            var query = new Query();
            //query.AddFileSource(new TestSearchSource(true));
            //query.AddFilter(new FileExtensionFilter("ts, js", FilterOperator.In));
            //query.AddFilter(new FileNameFilter("*.d.ts", FilterOperator.NotEqual));
            //query.AddFilter(new FileContentsFilter("canvascontext2d"));
            //query.AddFilter(new FileContentsFilter("circle"));

            var proc = new FileQueryProcessor();
            var results = proc.Execute(query);

        }
    }
}

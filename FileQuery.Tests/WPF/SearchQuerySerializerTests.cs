using FileQuery.Core.Filter;
using FileQuery.Wpf.Util;
using FileQuery.Wpf.ViewModels;
using NUnit.Framework;

namespace FileQuery.Tests.WPF
{
    [TestFixture]
    class SearchQuerySerializerTests
    {
        [Test]
        public void CanSerializeToYaml()
        {
            var vm = new SearchControlViewModel();
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Extension", ParamValue = "txt", ParamOperator = FilterOperatorUtil.GetOperatorItem("Extension", FilterOperator.Equal) });
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Contents", ParamValue = "test contents", ParamOperator = FilterOperatorUtil.GetOperatorItem("Contents", FilterOperator.NotEqual) });
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Modified Date", ParamValue = "9/8/2017", ParamOperator = FilterOperatorUtil.GetOperatorItem("Modified Date", FilterOperator.LessThan) });
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Name", ParamValue = "a, b, c", ParamOperator = FilterOperatorUtil.GetOperatorItem("Name", FilterOperator.In) });
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Read Only", ParamValue = "", ParamOperator = FilterOperatorUtil.GetOperatorItem("Read Only", FilterOperator.Equal) });
            vm.SearchParams.Add(new SearchParamItemViewModel { ParamType = "Size", ParamValue = "1KB", ParamOperator = FilterOperatorUtil.GetOperatorItem("Size", FilterOperator.GreaterThanEqual) });

            vm.SearchPaths.Add(new SearchPathItemViewModel { PathType = "Include-Recursive", PathValue = @"c:\recurse" });
            vm.SearchPaths.Add(new SearchPathItemViewModel { PathType = "Include-NoRecurse", PathValue = @"c:\norecurse" });
            vm.SearchPaths.Add(new SearchPathItemViewModel { PathType = "Exclude", PathValue = @"c:\exclude" });

            var yaml = SearchQuerySerializer.ToYaml(vm);
            Assert.AreEqual(filterYaml, yaml);
        }

        [Test]
        public void CanDeserializeYaml()
        {
            var vm = SearchQuerySerializer.FromYaml(filterYaml);
            Assert.NotNull(vm);
            Assert.AreEqual(6, vm.SearchParams.Count);
            Assert.AreEqual(3, vm.SearchPaths.Count);
        }

        const string filterYaml =
@"app: filequery
version: 2.0.1.0
paths:
- type: Include-Recursive
  value: c:\recurse
- type: Include-NoRecurse
  value: c:\norecurse
- type: Exclude
  value: c:\exclude
filters:
- type: Extension
  value: txt
  operator: Equal to
- type: Contents
  value: test contents
  operator: Does not contain text
- type: Modified Date
  value: 9/8/2017
  operator: Less than
- type: Name
  value: a, b, c
  operator: One of these values
- type: Read Only
  value: ''
  operator: True
- type: Size
  value: 1KB
  operator: Greater than or equal
";
    }
}

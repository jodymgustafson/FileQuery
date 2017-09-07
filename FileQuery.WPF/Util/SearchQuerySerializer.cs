using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FileQuery.Wpf.ViewModels;
using YamlDotNet.RepresentationModel;

namespace FileQuery.Wpf.Util
{
    class SearchQuerySerializer
    {
        public static string ToYaml(SearchControlViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("app: filequery")
              .Append("version: ").AppendLine(Assembly.GetExecutingAssembly().GetName().Version.ToString());

            sb.AppendLine("paths:");
            foreach (var path in model.SearchPaths)
            {
                sb.Append(" - type: ").AppendLine(path.PathType)
                  .Append("   value: ").AppendLine(path.PathValue);
            }

            sb.AppendLine("filters:");
            foreach (var filter in model.SearchParams)
            {
                sb.Append(" - type: ").AppendLine(filter.ParamType)
                  .Append("   operator: ").AppendLine(filter.ParamOperator.Label)
                  .Append("   value: ").AppendLine(filter.ParamValue);
            }

            return sb.ToString();
        }

        public static SearchControlViewModel FromYaml(string input)
        {
            return FromYaml(new StringReader(input));
        }

        public static SearchControlViewModel FromYaml(TextReader reader)
        {
            var model = new SearchControlViewModel();

            var yaml = new YamlStream();
            yaml.Load(reader);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            Validate(mapping);
            ParsePaths(model, mapping);
            ParseFilters(model, mapping);

            return model;
        }

        private static void Validate(YamlMappingNode mapping)
        {
            var name = ((YamlScalarNode)mapping.Children[new YamlScalarNode("app")]).Value;
            if (name != "filequery")
            {
                throw new Exception("This file doesn't appear to be compatible with File Query");
            }
        }

        private static void ParsePaths(SearchControlViewModel model, YamlMappingNode mapping)
        {
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("paths")];
            foreach (YamlMappingNode item in items)
            {
                model.SearchPaths.Add(new SearchPathItemViewModel
                {
                    PathType = item.Children[new YamlScalarNode("type")].ToString(),
                    PathValue = item.Children[new YamlScalarNode("value")].ToString(),
                });
            }
        }

        private static void ParseFilters(SearchControlViewModel model, YamlMappingNode mapping)
        {
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("filters")];
            foreach (YamlMappingNode item in items)
            {
                var filterType = item.Children[new YamlScalarNode("type")].ToString();
                var ops = FilterOperatorUtil.GetOperatorsForType(filterType);
                var opName = item.Children[new YamlScalarNode("operator")].ToString();
                var op = ops.FirstOrDefault(x => x.Label == opName);

                model.SearchParams.Add(new SearchParamItemViewModel
                {
                    ParamType = filterType,
                    ParamOperator = op,
                    ParamValue = item.Children[new YamlScalarNode("value")].ToString()
                });
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FileQuery.Wpf.ViewModels;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace FileQuery.Wpf.Util
{
    public static class SearchQuerySerializer
    {
        /// <summary>
        /// Converts a view model to yaml
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ToYaml(SearchControlViewModel model)
        {
            var root = new YamlMappingNode();
            root.Add("app", "filequery");
            root.Add("version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            var seq = new YamlSequenceNode();
            foreach (var path in model.SearchPaths.Where(x => x.IsValid))
            {
                var map = new YamlMappingNode();
                map.Add("type", path.PathType);
                map.Add("value", path.PathValue);
                seq.Add(map);
            }
            root.Add("paths", seq);

            seq = new YamlSequenceNode();
            foreach (var filter in model.SearchParams.Where(x => x.IsValid))
            {
                var map = new YamlMappingNode();
                map.Add("type", filter.ParamType);
                map.Add("value", filter.ParamValue);
                map.Add("operator", filter.ParamOperator.Label);
                seq.Add(map);
            }
            root.Add("filters", seq);

            var yaml = new SerializerBuilder().Build().Serialize(root);
            return yaml;
        }

        /// <summary>
        /// Converts yaml to a view model
        /// </summary>
        /// <param name="yaml"></param>
        /// <returns></returns>
        public static SearchControlViewModel FromYaml(string yaml)
        {
            return FromYaml(new StringReader(yaml));
        }

        /// <summary>
        /// Converts a yaml stream to a view model
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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

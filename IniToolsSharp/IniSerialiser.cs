using IniToolsSharp.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace IniToolsSharp
{
    public static class IniSerialiser
    {
        public static string Serialise<T>(T item) where T : notnull => Serialise(item, typeof(T));
        public static string Serialise(dynamic item, Type type)
        {
            var sections = GetSectionProperties(type);
            var sb = new StringBuilder();
            foreach (var section in sections)
                sb.Append(SerialiseSection(section.GetValue(item), section.GetCustomAttribute<IniSectionAttribute>()?.Name));
            return sb.ToString();
        }

        private static string SerialiseSection(object item, string sectionName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{sectionName}]");
            var props = item.GetType().GetProperties();
            foreach(var prop in props)
            {
                if (prop.GetCustomAttribute<IniIgnoreAttribute>() != null)
                    continue;
                sb.AppendLine($"{prop.Name}={prop.GetValue(item)}");
            }

            return sb.ToString();
        }

        public static T Deserialise<T>(string text) where T : notnull => Deserialise(text, typeof(T));
        public static dynamic Deserialise(string text, Type type)
        {
            text = text.Replace("\r","");
            var returnItem = Activator.CreateInstance(type);
            var sections = GetSectionProperties(type);
            foreach(var section in sections)
            {
                var attr = section.GetCustomAttribute<IniSectionAttribute>();
                if (attr != null)
                {
                    var targetSection = $"[{attr.Name}]";
                    var index = text.IndexOf(targetSection);
                    var end = text.IndexOf("\n[", index + targetSection.Length);
                    if (end == -1)
                        end = text.Length - index;
                    var sectionText = text.Substring(index, end - index);

                    var newSection = Activator.CreateInstance(section.PropertyType);
                    var sectionProps = section.PropertyType.GetProperties();
                    sectionText = sectionText.Remove(0, targetSection.Length);
                    var split = sectionText.Split('\n').ToList();
                    split.RemoveAll(x => x == "");
                    foreach(var line in split)
                    {
                        var equals = line.Split('=');
                        var left = equals[0];
                        var right = equals[1];
                        var target = sectionProps.First(x => x.Name == left);
                        if (target == null)
                            throw new IniDeserialisationException("Section does not contain stated values!");
                        target.SetValue(newSection, Convert.ChangeType(right.Trim(), target.PropertyType));
                    }

                    section.SetValue(returnItem, newSection);
                    text = text.Remove(index, end - index);
                }
            }

            return returnItem;
        }

        private static List<PropertyInfo> GetSectionProperties(Type type)
        {
            var propInfo = type.GetProperties();
            var columnProps = new List<PropertyInfo>();
            foreach (var prop in propInfo)
            {
                var attr = prop.GetCustomAttributes(false);
                if (attr.Any(x => x is IniSectionAttribute))
                    columnProps.Add(prop);
            }
            return columnProps;
        }
    }
}

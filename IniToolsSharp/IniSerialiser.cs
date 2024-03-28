using IniToolsSharp.Exceptions;
using System.Collections;
using System.Reflection;
using System.Text;

namespace IniToolsSharp
{
    /// <summary>
    /// Static class to handle serialisation and deserialisation of INI files
    /// </summary>
    public static class IniSerialiser
    {
        /// <summary>
        /// Serialise any item with <seealso cref="IniSectionAttribute"/> properties in it to a INI format
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="item">The item to serialise</param>
        /// <returns>A string in INI format</returns>
        public static string Serialise<T>(T item) where T : notnull => Serialise(item, typeof(T));
        /// <summary>
        /// Serialise any item with <seealso cref="IniSectionAttribute"/> properties in it to a INI format
        /// </summary>
        /// <param name="item">The item to serialise</param>
        /// <param name="type">Type of the item</param>
        /// <returns>A string in INI format</returns>
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
            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<IniIgnoreAttribute>() != null)
                    continue;
                var value = prop.GetValue(item);
                if (value is IEnumerable enu)
                    sb.AppendLine($"{prop.Name}={SerialiseEnumerable(enu)}");
                else
                    sb.AppendLine($"{prop.Name}={value}");
            }

            return sb.ToString();
        }

        private static string SerialiseEnumerable(IEnumerable items)
        {
            var str = "[";
            foreach (var item in items)
                str += $"{item},";
            if (str.EndsWith(','))
                str = str.Remove(str.Length - 1);
            str += "]";

            return str;
        }

        /// <summary>
        /// Deserialise a INI string into a object containing <seealso cref="IniSectionAttribute"/> properties
        /// </summary>
        /// <typeparam name="T">Type of the desired object</typeparam>
        /// <param name="text">Text in INI format</param>
        /// <returns>A new object of type <typeparamref name="T"/></returns>
        /// <exception cref="IniDeserialisationException"></exception>
        public static T Deserialise<T>(string text) where T : notnull => Deserialise(text, typeof(T));
        /// <summary>
        /// Deserialise a INI string into a object containing <seealso cref="IniSectionAttribute"/> properties
        /// </summary>
        /// <param name="text">Text in INI format</param>
        /// <param name="type">Type of the desired object</param>
        /// <returns>A new object of type <paramref name="type"/></returns>
        /// <exception cref="IniDeserialisationException"></exception>
        public static dynamic Deserialise(string text, Type type)
        {
            text = text.Replace("\r", "");
            var returnItem = Activator.CreateInstance(type);
            if (returnItem == null)
                throw new IniDeserialisationException($"Could not create a default instance of the type '{type.Name}'");
            var sections = GetSectionProperties(type);
            foreach (var section in sections)
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
                    var sectionProps = section.PropertyType.GetProperties().ToList();
                    sectionProps.RemoveAll(x => x.GetCustomAttribute<IniIgnoreAttribute>() != null);
                    sectionText = sectionText.Remove(0, targetSection.Length);
                    var split = sectionText.Split('\n').ToList();
                    split.RemoveAll(x => x == "");
                    foreach (var line in split)
                    {
                        var equals = line.Split('=');
                        var left = equals[0].Trim();
                        var right = equals[1].Trim();
                        var target = sectionProps.First(x => x.Name == left);
                        if (target == null)
                            throw new IniDeserialisationException($"Section '{attr.Name}' does not contain expected values!");

                        if (right.StartsWith('[') && right.EndsWith(']'))
                            target.SetValue(newSection, DeserialiseAsEnumerable(right, target.PropertyType));
                        else
                            target.SetValue(newSection, Convert.ChangeType(right, target.PropertyType));
                    }

                    section.SetValue(returnItem, newSection);
                    text = text.Remove(index, end - index);
                }
            }

            return returnItem;
        }

        private static object? DeserialiseAsEnumerable(string text, Type type)
        {
            text = text.Trim();
            text = text.Substring(1, text.Length - 2);
            var split = text.Split(',').ToList();
            split.RemoveAll(x => x == "");
            var target = Activator.CreateInstance(type);
            if (target is IList list)
            {
                foreach (var item in split)
                    list.Add(Convert.ChangeType(item, type.GenericTypeArguments[0]));
                return list;
            }
            throw new IniDeserialisationException($"Unknown target converation of list type: {type.Name}");
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

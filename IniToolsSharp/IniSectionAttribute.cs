namespace IniToolsSharp
{
    /// <summary>
    /// Attribute that describes a section in a INI file
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IniSectionAttribute : Attribute
    {
        /// <summary>
        /// Name of the section
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute that describes a section in a INI file
        /// </summary>
        /// <param name="name"></param>
        public IniSectionAttribute(string name)
        {
            Name = name;
        }
    }
}

namespace IniToolsSharp
{
    /// <summary>
    /// An attribute telling the Ini serialisation/deserialisation to ignore the given property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IniIgnoreAttribute : Attribute
    {
    }
}

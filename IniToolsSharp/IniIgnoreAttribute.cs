using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

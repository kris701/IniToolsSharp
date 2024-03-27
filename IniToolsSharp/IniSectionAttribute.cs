using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniToolsSharp
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IniSectionAttribute : Attribute
    {
        public string Name { get; set; }

        public IniSectionAttribute(string name)
        {
            Name = name;
        }
    }
}

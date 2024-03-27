using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniToolsSharp.Tests.TestClasses
{
    public class IniDocument2
    {
        [IniSection("SectionName")]
        public Section1 Section { get; set; } = new Section1();
        [IniSection("SectionName2")]
        public Section1 Section2 { get; set; } = new Section1();

        public override bool Equals(object? obj)
        {
            if (obj is IniDocument2 other)
            {
                if (!other.Section.Equals(Section)) return false;
                if (!other.Section2.Equals(Section2)) return false;
                return true;
            }
            return false;
        }
    }
}

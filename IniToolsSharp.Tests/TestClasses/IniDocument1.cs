using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniToolsSharp.Tests.TestClasses
{
    public class IniDocument1
    {
        [IniSection("SectionName")]
        public Section1 Section { get; set; } = new Section1();

        public override bool Equals(object? obj)
        {
            if (obj is IniDocument1 other)
            {
                if (!other.Section.Equals(Section)) return false;
                return true;
            }
            return false;
        }
    }
}

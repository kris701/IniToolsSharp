using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniToolsSharp.Tests.TestClasses
{
    public class Section2
    {
        public List<int> Values { get; set; } = new List<int>()
        {
            1,
            5,
            1341
        };

        public override bool Equals(object? obj)
        {
            if (obj is Section2 other)
            {
                if (other.Values.Count != Values.Count) return false;
                for (int i = 0; i < Values.Count; i++)
                    if (other.Values[i] != Values[i])
                        return false;
                return true;
            }
            return false;
        }
    }
}

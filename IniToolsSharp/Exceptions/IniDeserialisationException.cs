using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniToolsSharp.Exceptions
{
    public class IniDeserialisationException : Exception
    {
        public IniDeserialisationException(string? message) : base(message)
        {
        }
    }
}

namespace IniToolsSharp.Tests.TestClasses
{
    public class IniDocument3
    {
        [IniSection("SectionName")]
        public Section2 Section { get; set; } = new Section2();

        public override bool Equals(object? obj)
        {
            if (obj is IniDocument3 other)
            {
                if (!other.Section.Equals(Section)) return false;
                return true;
            }
            return false;
        }
    }
}

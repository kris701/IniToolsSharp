namespace IniToolsSharp.Tests.TestClasses
{
    public class Section1
    {
        public bool Value1 { get; set; } = false;
        public int Value2 { get; set; } = -1;

        public override bool Equals(object? obj)
        {
            if (obj is Section1 other)
            {
                if (other.Value1 != Value1) return false;
                if (other.Value2 != Value2) return false;
                return true;
            }
            return false;
        }
    }
}

namespace IniToolsSharp.Exceptions
{
    /// <summary>
    /// An exeption related to the deserialisation process
    /// </summary>
    public class IniDeserialisationException : Exception
    {
        /// <summary>
        /// An exeption related to the deserialisation process
        /// </summary>
        /// <param name="message">Exception message</param>
        public IniDeserialisationException(string? message) : base(message)
        {
        }
    }
}

namespace ParserXml.Model
{
    public interface IXmlElement
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string? Value { get; set; }
    }
}

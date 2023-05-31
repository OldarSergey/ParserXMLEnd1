namespace ParserXml.Model
{
    public class ElementStringRepresentation
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }

        public ElementStringRepresentation(string shortName, string lastName)
        {
            ShortName = shortName;
            FullName = lastName;
        }

        public override string ToString()
        {
            return $"{ShortName} : {FullName}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLParser.Model
{
	public class DTOXmlRepresentation
	{
		public string ShortName { get; set; }
		public string FullName { get; set; }
		public List<Model.Attribute> Attribute { get; set; }

		public DTOXmlRepresentation(string shortName, string fullName, List<Model.Attribute> attribute)
		{
			ShortName = shortName;
			FullName = fullName;
			Attribute = attribute;
		}
	}
}

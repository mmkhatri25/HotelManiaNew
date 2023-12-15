using System;

namespace CsvFiles
{
	public class CsvIgnorePropertyAttribute : Attribute
	{
		public override string ToString()
		{
			return "Ignore Property";
		}
	}
}

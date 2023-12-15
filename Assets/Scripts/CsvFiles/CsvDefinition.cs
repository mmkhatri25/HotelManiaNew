using System.Collections.Generic;

namespace CsvFiles
{
	public class CsvDefinition
	{
		public string Header
		{
			get;
			set;
		}

		public char FieldSeparator
		{
			get;
			set;
		}

		public char TextQualifier
		{
			get;
			set;
		}

		public IEnumerable<string> Columns
		{
			get;
			set;
		}

		public string EndOfLine
		{
			get;
			set;
		}

		public CsvDefinition()
		{
			if (CsvFile.DefaultCsvDefinition != null)
			{
				FieldSeparator = CsvFile.DefaultCsvDefinition.FieldSeparator;
				TextQualifier = CsvFile.DefaultCsvDefinition.TextQualifier;
				EndOfLine = CsvFile.DefaultCsvDefinition.EndOfLine;
			}
		}
	}
}

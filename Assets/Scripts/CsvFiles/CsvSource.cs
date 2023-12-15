using System.IO;

namespace CsvFiles
{
	public class CsvSource
	{
		public readonly TextReader TextReader;

		public static implicit operator CsvSource(CsvFile csvFile)
		{
			return new CsvSource(csvFile);
		}

		public static implicit operator CsvSource(string path)
		{
			return new CsvSource(path);
		}

		public static implicit operator CsvSource(TextReader textReader)
		{
			return new CsvSource(textReader);
		}

		public CsvSource(TextReader textReader)
		{
			TextReader = textReader;
		}

		public CsvSource(Stream stream)
		{
			TextReader = new StreamReader(stream);
		}

		public CsvSource(string path)
		{
			TextReader = new StreamReader(path);
		}

		public CsvSource(CsvFile csvFile)
		{
			TextReader = new StreamReader(csvFile.BaseStream);
		}
	}
}

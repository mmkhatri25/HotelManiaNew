using System.IO;

namespace CsvFiles
{
	public class CsvDestination
	{
		public StreamWriter StreamWriter;

		public static implicit operator CsvDestination(string path)
		{
			return new CsvDestination(path);
		}

		private CsvDestination(StreamWriter streamWriter)
		{
			StreamWriter = streamWriter;
		}

		private CsvDestination(Stream stream)
		{
			StreamWriter = new StreamWriter(stream);
		}

		public CsvDestination(string fullName)
		{
			FixCsvFileName(ref fullName);
			StreamWriter = new StreamWriter(fullName);
		}

		private static void FixCsvFileName(ref string fullName)
		{
			fullName = Path.GetFullPath(fullName);
			string directoryName = Path.GetDirectoryName(fullName);
			if (directoryName != null && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!string.Equals(Path.GetExtension(fullName), ".csv"))
			{
				fullName += ".csv";
			}
		}
	}
}

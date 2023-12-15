using System.Collections.Generic;

namespace CsvFiles
{
	public static class CsvFileLinqExtensions
	{
		public static void ToCsv<T>(this IEnumerable<T> source, CsvDestination csvDestination)
		{
			source.ToCsv(csvDestination, null);
		}

		public static void ToCsv<T>(this IEnumerable<T> source, CsvDestination csvDestination, CsvDefinition csvDefinition)
		{
			using (CsvFile<T> csvFile = new CsvFile<T>(csvDestination, csvDefinition))
			{
				foreach (T item in source)
				{
					csvFile.Append(item);
				}
			}
		}
	}
}

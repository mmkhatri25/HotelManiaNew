using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CsvFiles
{
	public class CsvFile : IDisposable
	{
		protected internal Stream BaseStream;

		protected static DateTime DateTimeZero;

		public static CsvDefinition DefaultCsvDefinition
		{
			get;
			set;
		}

		public static bool FastIndexOfAny
		{
			get;
			set;
		}

		public char FieldSeparator
		{
			get;
			private set;
		}

		public char TextQualifier
		{
			get;
			private set;
		}

		public IEnumerable<string> Columns
		{
			get;
			private set;
		}

		static CsvFile()
		{
			DefaultCsvDefinition = new CsvDefinition
			{
				EndOfLine = "\r\n",
				FieldSeparator = ',',
				TextQualifier = '"'
			};
			FastIndexOfAny = true;
		}

		public static IEnumerable<T> Read<T>(CsvSource csvSource) where T : new()
		{
			return new CsvFileReader<T>(csvSource);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
	public class CsvFile<T> : CsvFile
	{
		private readonly char fieldSeparator;

		private readonly string fieldSeparatorAsString;

		private readonly char[] invalidCharsInFields;

		private readonly StreamWriter streamWriter;

		private readonly char textQualifier;

		private readonly string[] columns;

		private Func<T, object>[] getters;

		private readonly bool[] isInvalidCharInFields;

		public CsvFile(CsvDestination csvDestination)
			: this(csvDestination, (CsvDefinition)null)
		{
		}

		public CsvFile()
		{
		}

		public CsvFile(CsvDestination csvDestination, CsvDefinition csvDefinition)
		{
			if (csvDefinition == null)
			{
				csvDefinition = CsvFile.DefaultCsvDefinition;
			}
			columns = (csvDefinition.Columns ?? InferColumns(typeof(T))).ToArray();
			fieldSeparator = csvDefinition.FieldSeparator;
			fieldSeparatorAsString = fieldSeparator.ToString(CultureInfo.InvariantCulture);
			textQualifier = csvDefinition.TextQualifier;
			streamWriter = csvDestination.StreamWriter;
			invalidCharsInFields = new char[4]
			{
				'\r',
				'\n',
				textQualifier,
				fieldSeparator
			};
			isInvalidCharInFields = new bool[256];
			char[] array = invalidCharsInFields;
			foreach (char c in array)
			{
				isInvalidCharInFields[c] = true;
			}
			WriteHeader();
			CreateGetters();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				streamWriter.Close();
			}
		}

		protected static IEnumerable<string> InferColumns(Type recordType)
		{
			return (from pi in recordType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				where pi.GetIndexParameters().Length == 0 && pi.GetSetMethod() != null && !Attribute.IsDefined(pi, typeof(CsvIgnorePropertyAttribute))
				select pi.Name).Concat(from fi in recordType.GetFields(BindingFlags.Instance | BindingFlags.Public)
				where !Attribute.IsDefined(fi, typeof(CsvIgnorePropertyAttribute))
				select fi.Name).ToList();
		}

		public void Append(T record)
		{
			string value = ToCsv(record);
			streamWriter.WriteLine(value);
		}

		private static Func<T, object> FindGetter(string c, bool staticMember)
		{
			BindingFlags bindingAttr = (BindingFlags)(0x31 | (staticMember ? 8 : 4));
			Func<T, object> result = null;
			PropertyInfo pi = typeof(T).GetProperty(c, bindingAttr);
			FieldInfo fi = typeof(T).GetField(c, bindingAttr);
			if (pi != null)
			{
				result = ((T o) => pi.GetValue(o, null));
			}
			else if (fi != null)
			{
				result = ((T o) => fi.GetValue(o));
			}
			return result;
		}

		private void CreateGetters()
		{
			List<Func<T, object>> list = new List<Func<T, object>>();
			string[] array = columns;
			foreach (string c in array)
			{
				Func<T, object> func = null;
				func = (FindGetter(c, staticMember: false) ?? FindGetter(c, staticMember: true));
				list.Add(func);
			}
			getters = list.ToArray();
		}

		private string ToCsv(T record)
		{
			if (record == null)
			{
				throw new ArgumentException("Cannot be null", "record");
			}
			string[] array = new string[getters.Length];
			for (int i = 0; i < getters.Length; i++)
			{
				object o = getters[i]?.Invoke(record);
				array[i] = ToCsvString(o);
			}
			return string.Join(fieldSeparatorAsString, array);
		}

		private string ToCsvString(object o)
		{
			if (o != null)
			{
				string text = (o as string) ?? Convert.ToString(o, CultureInfo.CurrentUICulture);
				if (RequiresQuotes(text))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(textQualifier);
					string text2 = text;
					foreach (char c in text2)
					{
						if (c == textQualifier)
						{
							stringBuilder.Append(c);
						}
						stringBuilder.Append(c);
					}
					stringBuilder.Append(textQualifier);
					return stringBuilder.ToString();
				}
				return text;
			}
			return string.Empty;
		}

		private bool RequiresQuotes(string valueString)
		{
			if (CsvFile.FastIndexOfAny)
			{
				int length = valueString.Length;
				for (int i = 0; i < length; i++)
				{
					char c = valueString[i];
					if (c <= 'ÿ' && isInvalidCharInFields[c])
					{
						return true;
					}
				}
				return false;
			}
			return valueString.IndexOfAny(invalidCharsInFields) >= 0;
		}

		private void WriteHeader()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < columns.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(fieldSeparator);
				}
				stringBuilder.Append(ToCsvString(columns[i]));
			}
			streamWriter.WriteLine(stringBuilder.ToString());
		}
	}
}

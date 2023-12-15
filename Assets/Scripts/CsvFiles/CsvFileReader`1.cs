using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CsvFiles
{
	internal class CsvFileReader<T> : CsvFile, IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : new()
	{
		private readonly Dictionary<Type, List<Action<T, string>>> allSetters = new Dictionary<Type, List<Action<T, string>>>();

		private string[] columns;

		private char curChar;

		private int len;

		private string line;

		private int pos;

		private T record;

		private readonly char fieldSeparator;

		private readonly TextReader textReader;

		private readonly char textQualifier;

		private readonly StringBuilder parseFieldResult = new StringBuilder();

		public T Current => record;

		public bool Eof => line == null;

		object IEnumerator.Current => Current;

		public CsvFileReader(CsvSource csvSource)
			: this(csvSource, (CsvDefinition)null)
		{
		}

		public CsvFileReader(CsvSource csvSource, CsvDefinition csvDefinition)
		{
			StreamReader streamReader = csvSource.TextReader as StreamReader;
			if (streamReader != null)
			{
				BaseStream = streamReader.BaseStream;
			}
			if (csvDefinition == null)
			{
				csvDefinition = CsvFile.DefaultCsvDefinition;
			}
			fieldSeparator = csvDefinition.FieldSeparator;
			textQualifier = csvDefinition.TextQualifier;
			textReader = csvSource.TextReader;
			ReadHeader(csvDefinition.Header);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				textReader.Dispose();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			ReadNextLine();
			if (line == null && (line = textReader.ReadLine()) == null)
			{
				record = default(T);
			}
			else
			{
				record = new T();
				Type typeFromHandle = typeof(T);
				if (!allSetters.TryGetValue(typeFromHandle, out List<Action<T, string>> value))
				{
					value = CreateSetters();
					allSetters[typeFromHandle] = value;
				}
				string[] array = new string[value.Count];
				for (int i = 0; i < value.Count; i++)
				{
					array[i] = ParseField();
					if (curChar != fieldSeparator)
					{
						break;
					}
					NextChar();
				}
				for (int j = 0; j < value.Count; j++)
				{
					value[j]?.Invoke(record, array[j]);
				}
			}
			return record != null;
		}

		public void Reset()
		{
			throw new NotImplementedException("Cannot reset CsvFileReader enumeration.");
		}

		private static Action<T, string> EmitSetValueAction(MemberInfo mi, Func<string, object> func)
		{
			PropertyInfo pi = mi as PropertyInfo;
			if (pi != null)
			{
				return delegate(T o, string v)
				{
					pi.SetValue(o, func(v), null);
				};
			}
			FieldInfo fi = mi as FieldInfo;
			if (fi != null)
			{
				return delegate(T o, string v)
				{
					fi.SetValue(o, func(v));
				};
			}
			throw new NotImplementedException();
		}

		private static Action<T, string> FindSetter(string c, bool staticMember)
		{
			BindingFlags bindingAttr = (BindingFlags)(0x31 | (staticMember ? 8 : 4));
			Action<T, string> result = null;
			PropertyInfo property = typeof(T).GetProperty(c, bindingAttr);
			if (property != null)
			{
				Func<string, object> func = StringToObject(property.PropertyType);
				result = EmitSetValueAction(property, func);
			}
			FieldInfo field = typeof(T).GetField(c, bindingAttr);
			if (field != null)
			{
				Func<string, object> func2 = StringToObject(field.FieldType);
				result = EmitSetValueAction(field, func2);
			}
			return result;
		}

		private static Func<string, object> StringToObject(Type propertyType)
		{
			if (propertyType == typeof(bool))
			{
				return (string s) => !string.IsNullOrEmpty(s) && Convert.ToBoolean(s, CultureInfo.InvariantCulture);
			}
			if (propertyType == typeof(string))
			{
				return (string s) => s ?? string.Empty;
			}
			if (propertyType == typeof(float))
			{
				return (string s) => string.IsNullOrEmpty(s) ? 0f : float.Parse(s, CultureInfo.InvariantCulture);
			}
			if (propertyType == typeof(int))
			{
				return (string s) => (!string.IsNullOrEmpty(s)) ? int.Parse(s) : 0;
			}
			if (propertyType == typeof(DateTime))
			{
				return (string s) => string.IsNullOrEmpty(s) ? CsvFile.DateTimeZero : DateTime.Parse(s);
			}
			throw new NotImplementedException();
		}

		private List<Action<T, string>> CreateSetters()
		{
			List<Action<T, string>> list = new List<Action<T, string>>();
			for (int i = 0; i < columns.Length; i++)
			{
				string text = columns[i];
				Action<T, string> action = null;
				if (text.IndexOf(' ') >= 0)
				{
					text = text.Replace(" ", "");
				}
				action = (FindSetter(text, staticMember: false) ?? FindSetter(text, staticMember: true));
				if (action == null)
				{
					UnityEngine.Debug.Log("csv: ignoring column: " + text);
				}
				list.Add(action);
			}
			return list;
		}

		private void NextChar()
		{
			if (pos < len)
			{
				pos++;
				curChar = ((pos < len) ? line[pos] : '\0');
			}
		}

		private void ParseEndOfLine()
		{
			throw new NotImplementedException();
		}

		private string ParseField()
		{
			parseFieldResult.Length = 0;
			if (line == null || pos >= len)
			{
				return null;
			}
			while (curChar == ' ' || curChar == '\t')
			{
				NextChar();
			}
			if (curChar == textQualifier)
			{
				NextChar();
				while (curChar != 0)
				{
					if (curChar == textQualifier)
					{
						NextChar();
						if (curChar != textQualifier)
						{
							return parseFieldResult.ToString();
						}
						NextChar();
						parseFieldResult.Append(textQualifier);
					}
					else if (curChar == '\0')
					{
						if (line == null)
						{
							return parseFieldResult.ToString();
						}
						ReadNextLine();
					}
					else
					{
						parseFieldResult.Append(curChar);
						NextChar();
					}
				}
			}
			else
			{
				while (curChar != 0 && curChar != fieldSeparator && curChar != '\r' && curChar != '\n')
				{
					parseFieldResult.Append(curChar);
					NextChar();
				}
			}
			return parseFieldResult.ToString();
		}

		private void ReadHeader(string header)
		{
			if (header == null)
			{
				ReadNextLine();
			}
			else
			{
				line = header;
				pos = -1;
				len = line.Length;
				NextChar();
			}
			List<string> list = new List<string>();
			string item;
			while ((item = ParseField()) != null)
			{
				list.Add(item);
				if (curChar != fieldSeparator)
				{
					break;
				}
				NextChar();
			}
			columns = list.ToArray();
		}

		private void ReadNextLine()
		{
			line = textReader.ReadLine();
			pos = -1;
			if (line == null)
			{
				len = 0;
				curChar = '\0';
			}
			else
			{
				len = line.Length;
				NextChar();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.Scripts
{
	public class Config
	{

		public Dictionary<string, List<Property>> Properties;

		public Config ()
		{
			Properties = new Dictionary<string, List<Property>>();
		}

		public void ReadConfig (string filePath)
		{
			try
			{
				List<string> lines = new List<string>(File.ReadAllLines(filePath));
				lines = cleanLines(lines);
				collectHeaders(lines);
			}
			catch (IOException ioe)
			{
				throw new Exception("Could not read file", ioe);
			}
		}

		private List<string> cleanLines (List<string> rawLines)
		{
			int c = rawLines.Count;
			for (int i = 0; i < c; i++)
			{
				string line = rawLines[i];
				if (line == "" || line[0] == ';')
				{
					rawLines.RemoveAt(i);
					i--;
					c--;
				}
			}

			return rawLines;
		}

		private void collectHeaders (List<string> cleanLines)
		{
			string currentHeader = "";

			int i = 0;
			while (cleanLines.Count > 0)
			{
				string line = cleanLines[0];

				if (line[0] == '[')
				{
					currentHeader = getHeader(line);
					Properties.Add(currentHeader, new List<Property>());
				}
				else
				{
					if (currentHeader == "") continue;

					collectProperty(line, currentHeader);
				}

				cleanLines.Remove(line);
			}
		}

		private string getHeader (string line)
		{
			return line.Substring(1, line.Length - 2);
		}

		private void collectProperty (string line, string header)
		{
			string[] parts = line.Split('=');

			if (parts.Length < 2) return;

			Properties[header].Add(new Property(parts[0], parts[1]));
		}

		public struct Property
		{
			public string Key;
			public string Value;

			public Property(string key, string value)
			{
				Key = key;
				Value = value;
			}

			public string ToString ()
			{
				return String.Format("key: {0}, value: {1}", Key, Value);
			}
		}

	}
}

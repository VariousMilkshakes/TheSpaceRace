using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

namespace SpaceRace.Utils
{
	public class Config
	{
		public static Dictionary<string, Config> LOAD ()
		{
			TextAsset[] assetArray = Resources.LoadAll<TextAsset>("Config/");
			Dictionary<string, Config> configs = new Dictionary<string, Config>();

			foreach (TextAsset ta in assetArray)
			{
				Config c = new Config();
				try
				{
					c.ReadConfig(ta);
				}
				catch
				{
					continue;
				}

				configs.Add(c.ConfigHeader, c);
			}

			return configs;
		}


		public Dictionary<string, List<Property>> Properties;

		public string ConfigHeader { get { return cHeader; } }
		private string cHeader = "";

		public Config ()
		{
			Properties = new Dictionary<string, List<Property>>();
		}

		public void ReadConfig (TextAsset textFile)
		{
			List<string> lines = textFile.text.Split('\n').ToList<string>();
			lines = cleanLines(lines);
			collectHeaders(lines);
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

				line = line.Replace("\r", "");
				rawLines[i] = line;

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
					// First Header is used to indenitify config file
					if (cHeader == "")
					{
						cHeader = currentHeader;
					}
					else
					{
						Properties.Add(currentHeader, new List<Property>());
					}
				}
				else
				{
					if (currentHeader != "")
						collectProperty(line, currentHeader);
				}

				cleanLines.Remove(line);
			}
		}

		/// <summary>
		/// Check if object has relevant config file
		/// </summary>
		/// <param name="t">Type of building to look for config of</param>
		/// <returns>Relevant config</returns>
		public Property LookForProperty(string configHeader, string propKey)
		{
			foreach (Property p in Properties[configHeader])
			{
				if (p.IsProperty(propKey)) return p;
			}

			throw new Exception("Could not find property in config file: " + propKey);
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

			public bool IsProperty (string checkKey)
			{
				if (checkKey == Key) return true;
				return false;
			}
		}

	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpaceRace.Utils;

namespace Assets.Scripts.Tests
{
	[TestClass()]
	public class ConfigTests
	{
		public Config config;

		[TestMethod()]
		public void ConfigTest()
		{
			config = new Config();
			Assert.IsNotNull(config.Properties);
			Config.LOAD();
		}

		[TestMethod()]
		public void ReadConfigTest()
		{
			config = new Config();

			try
			{
				config.ReadConfig(Resources);
			}
			catch (System.IO.IOException ioe)
			{
				Console.WriteLine(ioe);
				Assert.Fail();
			}
			
			Assert.IsNotNull(config.Properties["House"]);
			Config.Property p = config.Properties["House"][0];
			Console.WriteLine(p.ToString());
			Assert.AreEqual(p.Value, "Wood");
		}
	}
}
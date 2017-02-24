using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceRace.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceRace.Utils.Tests
{
    [TestClass()]
    public class Config_Tests
    {
        [TestMethod()]
        public void LOAD_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Config_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadConfig_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPropertyResourceBox_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LookForProperty_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Property_Test ()
        {
            Property property = new Property("key", "value");
            Assert.AreEqual("key", property.Key);
            Assert.AreEqual("value", property.Value);
        }

        [TestMethod ()]
        public void IsProperty_Test ()
        {
            Property property = new Property("key", "value");
            Assert.AreEqual(true, property.IsProperty("key"));
            Assert.AreEqual(false, property.IsProperty("boo"));
        }
    }
}
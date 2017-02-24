using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRace.Utils
{
    public struct Property
    {
        public string Key;
        public string Value;

        public Property(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string ToString()
        {
            return String.Format("key: {0}, value: {1}", Key, Value);
        }

        public bool IsProperty(string checkKey)
        {
            if (checkKey == Key) return true;
            return false;
        }
    }
}

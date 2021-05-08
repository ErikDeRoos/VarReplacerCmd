using System;
using System.Collections.Generic;

namespace VarReplacerCmd.Replace
{
    public class EnvironmentLookupCached : IReferenceLookup
    {
        private readonly Dictionary<string, string> _lookup = new Dictionary<string, string>();

        public string Lookup(string varName)
        {
            var name = GetEnvironmentName(varName);

            if (_lookup.TryGetValue(name, out var value))
                return value;

            value = Environment.GetEnvironmentVariable(name);
            _lookup.Add(name, value);

            return value;
        }

        private static string GetEnvironmentName(string varName)
        {
            return varName.ToUpper().Replace('.', '_');
        }
    }
}

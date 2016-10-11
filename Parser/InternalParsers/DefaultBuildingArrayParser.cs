using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Isometric.Core.Modules.WorldModule;

namespace Isometric.Parser.InternalParsers
{
    public class DefaultBuildingArrayParser// : IParser
    {
        private DefaultBuildingArrayParser _instance;
        public DefaultBuildingArrayParser Instance => _instance ?? (_instance = new DefaultBuildingArrayParser());

        private DefaultBuildingArrayParser() { }



        public Type Type => typeof (World.DefaultBuilding[]);



        public bool TryParse(string str, object additionalData, out object obj)
        {
            throw new NotImplementedException();
            //obj = null;
            //var result = new List<World.DefaultBuilding>();

            //if (!Regex.IsMatch(str, @"^ *(\w*: *\d{1,},? *)*(\w*: *\d{1,})? *$"))
            //{
            //    return false;
            //}
        }
    }
}
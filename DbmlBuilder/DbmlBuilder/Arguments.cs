using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace DbmlBuilder
{
    /// <summary>
    /// Command Line Parsing Library.
    /// </summary>
    public class Arguments
    {
        // Variables
        private readonly StringDictionary parameters = new StringDictionary();

        // Constructor
        public Arguments(string commandLine)
        {
            string pattern = @"/(?<arg>((?!/).)*?)(\s*""(?<value>[^""]*)""|\s+(?<value>(?!/).*?)([\s]|$)|(?<value>\s+))";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(commandLine);
            foreach(Match match in matches)
            {
                string arg = match.Groups["arg"].Value;
                string value = match.Groups["value"].Value;
                parameters.Add(arg, value);
            }
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string Param]
        {
            get
            {
                return (parameters[Param]);
            }
        }
    }
}

using System;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DbmlBuilder.Utilities;

namespace DbmlBuilder
{
    public class StoredProcedure
    {
        public class Parameter
        {
            public string SqlType { get; set; }
            private DbType dbType;

            public DbType DBType
            {
                get { return dbType; }
                set { dbType = value; }
            }

	
            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private string displayName;

            public string DisplayName
            {
                get { return displayName; }
                set { displayName = value; }
            }

            private string queryParameter;

            public string QueryParameter
            {
                get { return queryParameter; }
                set { queryParameter = value; }
            }

            private ParameterDirection mode = ParameterDirection.Input;

            public ParameterDirection Mode
            {
                get { return mode; }
                set { mode = value; }
            }
        }

        public class ParameterCollection : List<Parameter>
        {
        }

        private ParameterCollection _parameters = new ParameterCollection();

        public ParameterCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public List<object> OutputValues;

        public string ReturnType { get; set; }

        private int commandTimeout = 60;

        public int CommandTimeout 
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        public string sqlType { get; set; }
        public DbType dbType { get; set; }

        public string SchemaName { get; set; }

        private readonly string name;

        public string Name
        {
            get { return name; }
        }

        private readonly string displayName;

        public string DisplayName
        {
            get { return displayName; }
        }

        public string ClassName { get; set; }

        public StoredProcedure(string schema, string spName)
        {
            name = spName;
            this.SchemaName = schema;
            string newName = spName;
            displayName = TransformSPName(newName);
            //init the list so the count comes back properly
            OutputValues = new List<object>();
        }

        private static string TransformSPName(string spName)
        {
            if (String.IsNullOrEmpty(spName))
                return string.Empty;

            string newName = spName;
            newName = Utility.GetProperName(newName);
            newName = Utility.IsStringNumeric(newName) ? "_" + newName : newName;
            newName = Utility.StripNonAlphaNumeric(newName);
            newName = newName.Trim();
            return Utility.KeyWordCheck(newName, String.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbmlBuilder
{

    internal class AutomaticConnectionScope
    {
        private readonly SqlConnection _dbConnection;

        internal SqlConnection Connection
        {
            get { return _dbConnection; }
        }

        internal AutomaticConnectionScope()
        {
            _dbConnection = Db.Service.CreateConnection();
            _dbConnection.Open();
        }
    }
}

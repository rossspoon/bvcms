using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DbmlBuilder
{

    internal class AutomaticConnectionScope
    {
        private readonly DbConnection _dbConnection;

        internal DbConnection Connection
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

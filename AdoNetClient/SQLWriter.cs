using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AdoNetClient
{
    class SQLWriter
    {
        public SQLWriter(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        IUserInteractor userInteractor;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fitlibrary;
using System.Data.SqlClient;
using TddWithFitnesse.TransactionHelper;
using System.Diagnostics;

namespace TddWithFitnesse.Acceptance
{
    public class AdoDoFixture : DoFixture
    {
        private readonly string ConnectionString = @"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes";

        public void StartTransaction()
        {
            CommonSessionManager.Connection = new SqlConnection(ConnectionString);
            CommonSessionManager.Connection.Open();
            CommonSessionManager.Transaction = CommonSessionManager.Connection.BeginTransaction();
        }

        public void EndTransaction()
        {
            CommonSessionManager.Transaction.Rollback();
            CommonSessionManager.Transaction.Dispose();
            CommonSessionManager.Connection.Close();
            CommonSessionManager.Connection.Dispose();
        }

        public void Debug()
        {
            Debugger.Launch();
        }
    }
}

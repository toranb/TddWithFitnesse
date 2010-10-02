using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TddWithFitnesse.TransactionHelper
{
    public class CommonSessionManager : IHttpModule
    {
        public static string ConnectionString = @"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes";
        public static IDbConnection Connection;
        public static IDbTransaction Transaction;

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Application_BeginRequest;
            context.EndRequest += Application_EndRequest;
        }

        private static void Application_BeginRequest(object sender, EventArgs e)
        {
            SetupAdoConnection();
            SetupAdoTransaction();
        }

        private static void Application_EndRequest(object sender, EventArgs e)
        {
            CommitTransactionAndCloseConnection();
        }

        private static void SetupAdoConnection()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        private static void SetupAdoTransaction()
        {
            if (Transaction != null && Transaction.Connection != null && Connection != null && Connection.State == ConnectionState.Open)
                Transaction = Connection.BeginTransaction();
        }

        private static void CommitTransactionAndCloseConnection()
        {
            if (Transaction != null && Transaction.Connection != null && Connection != null && Connection.State == ConnectionState.Open)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Connection.Close();
                Connection.Dispose();
            }
        }

        public void Dispose() { }
    }
}
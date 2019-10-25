using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using System;
using System.Data.SqlClient;
using System.Text;
using Serilog;
using System.Data;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestDatabase
    {
        private static readonly string connStr = @"Data Source=DESKTOP-SK7GEOP\SQLEXPRESS;Database=Project0;Integrated Security=true";
        private static bool connected = false;
        private static SqlConnection conn;
        private static void Connect()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open) return;
            conn = new SqlConnection(connStr);
            conn.Open();
        }


        public static SqlDataReader ReadCommand(string cmd)
        {
            Connect();
            return new SqlCommand(cmd, conn).ExecuteReader();
        }

        public static int DoCommand(string cmd)
        {
            Connect();

            return new SqlCommand(cmd, conn).ExecuteNonQuery();
        }

        [TestMethod]
        public void Execute(string cmd, string table)
        {
            UnitTestSetup.SetupTesting();
            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, table);

            DataTable dt = ds.Tables[table];
            StringBuilder sb = new StringBuilder();
            foreach(DataRow row in dt.Rows)
            {
                foreach(var item in row.ItemArray)
                {
                    sb.Append($"{item} ");
                }
                sb.Append("\n");
            }
        }

        [TestMethod]
        public void TestReadCustomers()
        {
            UnitTestSetup.SetupTesting();
            SqlDataReader r = ReadCommand("SELECT * FROM Customers");
            while (r.Read())
            {
                Console.WriteLine($"{r[0]} {r[1]} {r[2]} {r[3]}");
            }
        }

        [TestMethod]
        public void TestManualInsertCustomer()
        {
            UnitTestSetup.SetupTesting();
            string username = "user";
            byte[] hash = new byte[64];
            byte[] salt = new byte[36];
            (new Random()).NextBytes(hash);
            (new Random()).NextBytes(salt);
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Customers (Username, PasswordHash, Salt) ");
            sb.Append("VALUES('");
            sb.Append(username); sb.Append("','");
            sb.Append(Encoding.UTF8.GetString(hash)); sb.Append("','");
            sb.Append(Encoding.UTF8.GetString(salt)); sb.Append("')");
            int aff = DoCommand(sb.ToString());
            Log.Information($"TestInsertCustomer: {aff} rows affected.");
            Assert.IsTrue(aff == 1);
        }
    }
}

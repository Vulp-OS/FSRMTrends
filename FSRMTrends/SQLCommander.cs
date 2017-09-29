using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FSRMTrends
{
    class SQLCommander
    {
        private static DataTable ReadSQLCommand(string queryString, string connectionString)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    SqlDataAdapter dsa = new SqlDataAdapter(command);
                    dsa.Fill(dt);
                }
            } catch (Exception e)
            {
                MessageBox.Show("Please correct the connection string in Settings->Options");
            }
            return dt;
        }

        private static void WriteSQLCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static String getConnString()
        {
            return SQLDataSource.Default.ConnectionString; ;
        }

        public static ArrayList getQuotaNames()
        {
            ArrayList quotaNames = new ArrayList();

            String query = "SELECT * FROM QuotaPaths";
            DataTable temp = ReadSQLCommand(query, getConnString());
            
            quotaNames.AddRange(temp.AsEnumerable().Select(row => row.Field<string>("QuotaPath")).ToArray());

            return quotaNames;
        }

        public static Dictionary<String, Dictionary<String, Object>> getQuotaUsages()
        {
            Dictionary<String, Dictionary<String, Object>> quotaUsages = new Dictionary<String, Dictionary<String, Object>>();
            String query = "SELECT * from Usages";

            DataTable temp = ReadSQLCommand(query, getConnString());

            IEnumerable<DataRow> tempEnum = temp.AsEnumerable();
            foreach (DataRow row in tempEnum)
            {
                Dictionary<String, Object> properties = new Dictionary<string, object>();
                properties.Add("Date", row.Field<DateTime>(1));
                properties.Add("Usage", (UInt64)row.Field<Decimal>(2));

                quotaUsages.Add(row.Field<String>(0), properties);
            }

            return quotaUsages;
        }

        public static ArrayList getQuotaUsagesByPath(String path)
        {
            ArrayList quotaUsages = new ArrayList();
            String query = "SELECT * from Usages WHERE QuotaPath='" + path + "' ORDER BY Date ASC";

            DataTable temp = ReadSQLCommand(query, getConnString());
            IEnumerable<DataRow> tempEnum = temp.AsEnumerable();

            foreach (DataRow row in tempEnum)
            {
                ArrayList property = new ArrayList();

                property.Add(row.Field<DateTime>(1));
                property.Add((UInt64)row.Field<Decimal>(2));
                quotaUsages.Add(property);
            }

            return quotaUsages;
        }

        public static ArrayList getQuotaSizesByPath(String path)
        {
            ArrayList quotaSizes = new ArrayList();
            String query = "SELECT * from Sizes WHERE QuotaPaths='" + path + "' ORDER BY Date ASC";

            DataTable temp = ReadSQLCommand(query, getConnString());
            IEnumerable<DataRow> tempEnum = temp.AsEnumerable();

            foreach (DataRow row in tempEnum)
            {
                ArrayList property = new ArrayList();

                property.Add(row.Field<DateTime>(1));
                property.Add((UInt64)row.Field<Decimal>(2));
                quotaSizes.Add(property);
            }

            return quotaSizes;
        }

        public static Dictionary<String, Dictionary<String, Object>> getQuotaSizes()
        {
            Dictionary<String, Dictionary<String, Object>> quotaSizes = new Dictionary<String, Dictionary<String, Object>>();
            String query = "SELECT * from Sizes";

            DataTable temp = ReadSQLCommand(query, getConnString());

            IEnumerable<DataRow> tempEnum = temp.AsEnumerable();
            foreach (DataRow row in tempEnum)
            {
                Dictionary<String, Object> properties = new Dictionary<string, object>();
                properties.Add("Date", row.Field<DateTime>(1));
                properties.Add("Usage", (UInt64)row.Field<Decimal>(2));

                quotaSizes.Add(row.Field<String>(0), properties);
            }

            return quotaSizes;
        }

        public static void updateSQLDatabase()
        {
            //get timenow for inserting into DATES table
            DateTime timenow = DateTime.Now;
            //timenow.ToString("yyyy-MM-dd HH:mm:ss");

            //get FSRM quotas from WMI
            FSRMQuery FSRMQ = new FSRMQuery();
            ArrayList queryProperties = FSRMQ.getFSRMData();

            //Parse desired data from WMI results and insert to SQL
            String dateCommand = "INSERT INTO Dates VALUES ((SELECT CONVERT(datetime, '" + timenow + "', 120)))";
            WriteSQLCommand(dateCommand, getConnString());

            foreach (Dictionary<String, Object> properties in queryProperties)
            {
                String path = properties["Path"].ToString();
                UInt64 usage;
                UInt64.TryParse(properties["Usage"].ToString(), out usage);
                UInt64 size;
                UInt64.TryParse(properties["Size"].ToString(), out size);

                //Only use to populate an empty database
                //String sqlCommand = "INSERT INTO QuotaPaths VALUES ('" + path + "')";
                //WriteSQLCommand(sqlCommand, getConnString());

                String usagesCommand = "INSERT INTO Usages VALUES ('" + path + "', (SELECT CONVERT(datetime, '" + timenow + "', 120)), " + usage + ")";
                String sizesCommand = "INSERT INTO Sizes VALUES ('" + path + "', (SELECT CONVERT(datetime, '" + timenow + "', 120)), " + size + ")";

                WriteSQLCommand(usagesCommand, getConnString());
                WriteSQLCommand(sizesCommand, getConnString());
            }
        }

        public static void updatePathList()
        {
            //get FSRM quotas from WMI
            FSRMQuery FSRMQ = new FSRMQuery();
            ArrayList queryProperties = FSRMQ.getFSRMData();

            //Get existing paths from SQL
            String listPathsCommand = "SELECT * FROM QuotaPaths";
            DataTable paths = ReadSQLCommand(listPathsCommand, getConnString());

            //Convert DataTable to ArrayList
            ArrayList pathsArray = new ArrayList();
            pathsArray.AddRange(paths.AsEnumerable().Select(row => row.Field<string>("QuotaPath")).ToArray());

            //Add paths from WMI to SQL if they don't already exist
            foreach (Dictionary<String, Object> properties in queryProperties)
            {
                String path = properties["Path"].ToString();

                if (!pathsArray.Contains(path))
                {
                    String sqlCommand = "INSERT INTO QuotaPaths VALUES ('" + path + "')";
                    WriteSQLCommand(sqlCommand, getConnString());
                }
            }
        }
    }
}

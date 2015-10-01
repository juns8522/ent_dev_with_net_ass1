using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using BlueConsultingDB.ReportsTableAdapters;

namespace BlueConsultingDB
{
    public class User
    {
        public string Username { get; set; }
        public string Department { get; set; }
        public String UserId { get; set; }

        protected string connectionString;

        public User()
        {
        }

        public User(string userId)
        {
            // Initialise db connection
            this.connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

            // User object fields
            this.UserId = userId;
            GetUserDetails();

            // Initialise path to database
            string pathToData = "../BlueConsultingDB";
            string dataDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToData));
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
        }

        /**
         * Fetch the user details
         */
        public void GetUserDetails()
        {
            try
            {
                // Database command
                String strCommand = @"SELECT u.UserName FROM aspnet_Users u WHERE u.UserId = @USerId";

                // Open the db connection
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                // SQL Command
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = strCommand;
                cmd.Parameters.AddWithValue("@UserId", this.UserId);

                // Fetch the data
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);
                DataTable table = dataset.Tables[0];
                DataRow userRow = table.Rows[0];
                this.Username = userRow["UserName"].ToString();
                this.Department = GetDepartment();
            }

            // Unable to load the user
            catch (Exception e)
            {
            }
        }


        public virtual Reports.ReportsDataTable GetReports(int type)
        {
            return null;
        }

        /**
         * Fetch a user department
         */
        protected string GetDepartment()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Department FROM Departments WHERE UserId = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", UserId);
            connection.Open();

            String department = command.ExecuteScalar().ToString();
            connection.Close();

            return department;
        }
    }
}

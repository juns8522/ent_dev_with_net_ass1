using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using BlueConsultingDB.ReportsTableAdapters;

namespace BlueConsultingDB
{
    /*
     * The Class for accounts staff in Blue Consulting.
     */
    public class AccountsStaff : User
    {
        public double DeptBudget;
        public double TotalBudget;

        public double StateBudgetRemained { get; set; }
        public double LogisticsBudgetRemained { get; set; }
        public double HigherEducationBudgetRemained { get; set; }
        public double StateExpenses { get; set; }
        public double LogisticsExpenses { get; set; }
        public double HigherEducationExpenses { get; set; }
        public double TotalBudgetRemained { get; set; }
        public double TotalExpenses { get; set; }

        public AccountsStaff(string userId) : base(userId)
        {
            this.DeptBudget = Convert.ToDouble(ConfigurationManager.AppSettings["DepartmentBudget"].ToString());
            this.TotalBudget = Convert.ToDouble(ConfigurationManager.AppSettings["CompanyBudget"].ToString()); ;
        }

        /*
         * Supervisors display reports approved by supervisors.
         * This method will return the value as ReportsDataTable.
         * So that this object will be binded with GridView in UI project.
         */
        public override Reports.ReportsDataTable GetReports(int type)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.FillByAccounts(reportsTable);
            
            return reportsTable;
        }

        /*
         * This will determine that a specific report is already approved/rejected by accounts staff or not
         * approved/rejected = false
         * still processing = ture
         */
        public bool IsProcessing(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.FillById(reportsTable, reportId);
            Reports.ReportsRow row = (Reports.ReportsRow)reportsTable.Rows[0];

            if (row.Accounts_approval.Equals("PROCESSING"))
                return true;
            else
                return false;
        }

        /*
         * this will update the value of column Accounts_approval in Reports table
         * with value "APPROVED"
         */
        public bool Approve(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.UpdateAccApprove(reportId);
            return true;
        }

        /*
         * this will update the value of column Accounts_approval in Reports table
         * with value "REJECTED"
         */
        public bool Reject(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.UpdateAccReject(reportId);
            return true;
        }

        /*
         * This will calulate total expenses and remained budget of company.
         */
        public void CalculateExpenses()
        {
            CalculateDeptExpenses("State Services");
            CalculateDeptExpenses("Logistics Services");
            CalculateDeptExpenses("Higher Education Services");

            TotalExpenses = StateExpenses + LogisticsExpenses + HigherEducationExpenses;
            TotalBudgetRemained = TotalBudget - TotalExpenses;
        }

        /*
         * This will calulate total expenses and remained budget of each department.
         * the espenses are those approved by both supervisor and accounts staff
         */
        private void CalculateDeptExpenses(string department)
        {
            DateTime today = DateTime.Today;
            string startOfMonth = new DateTime(today.Year, today.Month, 1).ToString();
            
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            switch (department)
            {
                case "State Services":
                    adapter.FillByExpensesBothApproved(reportsTable, department, startOfMonth);
                    foreach (Reports.ReportsRow row in reportsTable)
                    {
                        StateExpenses += row.Total_amount;
                    }
                    StateBudgetRemained = DeptBudget - StateExpenses;
                    break;
                case "Logistics Services":
                    adapter.FillByExpensesBothApproved(reportsTable, department, startOfMonth);
                    foreach (Reports.ReportsRow row in reportsTable)
                    {
                        LogisticsExpenses += row.Total_amount;
                    }
                    LogisticsBudgetRemained = DeptBudget - LogisticsExpenses;
                    break;
                case "Higher Education Services":
                    adapter.FillByExpensesBothApproved(reportsTable, department, startOfMonth);
                    foreach (Reports.ReportsRow row in reportsTable)
                    {
                        HigherEducationExpenses += row.Total_amount;
                    }
                    HigherEducationBudgetRemained = DeptBudget - HigherEducationExpenses;
                    break;
            }
        }

        /*
         * Resets calculated values
         */
        public void ResetBudget()
        {
            StateBudgetRemained = 0;
            LogisticsBudgetRemained = 0;
            HigherEducationBudgetRemained = 0;
            StateExpenses = 0;
            LogisticsExpenses = 0;
            HigherEducationExpenses = 0;
            TotalBudgetRemained = 0;
            TotalExpenses = 0; 
        }

        /*
         * This calculates total expenses approved by individual supervisors
         */
        public DataTable IndividualExpenses()
        {
            DateTime today = DateTime.Today;
            string startOfMonth = new DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd");
            string strCommand = "select u.UserName as Supervisor, sum(Total_amount) as Expenses from Reports AS r " +
                                "left join aspnet_Users AS u " +
                                "on r.Supervisor_id = u.UserId " +
                                "where r.Supervisor_approval = 'APPROVED' and r.Accounts_approval = 'APPROVED' " +
                                "and r.Date >= '" + startOfMonth + "' " +
                                "group by u.UserName";
            string connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(strCommand, connection);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            DataTable table = dataset.Tables[0];
            connection.Close();
            
            return table;
        }

        /*
        * This will all expenses of selected report.
        */
        public Reports.ExpensesDataTable GetExpenses(int reportId)
        {
            ExpensesTableAdapter adapter = new ExpensesTableAdapter();
            Reports.ExpensesDataTable expensesTable = new Reports.ExpensesDataTable();

            adapter.FillByReportId(expensesTable, reportId);
            return expensesTable;
        }

        /*
         * This will retreive the Receipt data from Expenses table and return it.
         */
        public byte[] GetPdf(int pdfData)
        {
            string query = "Select Receipt, Id from Expenses where Id = @id";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", pdfData);
            Byte[] pdf = (byte[])command.ExecuteScalar();
            connection.Close();
            return pdf;
        }
    }
}

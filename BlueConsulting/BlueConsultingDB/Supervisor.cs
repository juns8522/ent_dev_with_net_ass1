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
    /*
     * The Class for supervisors in Blue Consulting.
     */
    public class Supervisor : User
    {
        public double MonthlyBudget;
        
        public double Expenses { get; set; }
        public double BudgetRemained { get; set; }

        public Supervisor(string userId) : base(userId)
        {
            this.MonthlyBudget = Convert.ToDouble(ConfigurationManager.AppSettings["DepartmentBudget"].ToString());
        }

        /*
         * Supervisors display all reports of their department, the reports rejected by 
         * accounts staff. This method will return the value as ReportsDataTable.
         * So that this object will be binded with GridView in UI project.
         */
        public override Reports.ReportsDataTable GetReports(int type)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            switch (type)
            {
                case 1:
                    adapter.FillByDepartment(reportsTable, Department);
                    break;
                case 2:
                    adapter.FillByRejected(reportsTable, Department);
                    break;
            }
            return reportsTable;
        }

        /*
         * This will determine that expenses of a report will make company over budget or not
         * over budet = true, no = false
         */
        public bool IsOverBudget(int reportId)
        {
            CalculateExpenses();
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.FillById(reportsTable, reportId);
            Reports.ReportsRow row = (Reports.ReportsRow)reportsTable.Rows[0];
            if (row.Total_amount > BudgetRemained)
            {
                ResetBudget();
                return true;
            }
            ResetBudget();
            return false;
        }

        /*
         * This will determine that a specific report is already approved/rejected by supervisor or not
         * approved/rejected = false
         * still processing = ture
         */
        public bool IsProcessing(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.FillById(reportsTable, reportId);
            Reports.ReportsRow row = (Reports.ReportsRow)reportsTable.Rows[0];

            if(row.Supervisor_approval.Equals("PROCESSING"))
                return true;
            else
                return false;
        }

        /*
         * this will update the value of column Supervisor_approval in Reports table
         * with value "APRROVED"
         */
        public bool Approve(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.UpdateSupApprove(UserId, reportId);
            return true;
        }

        /*
         * this will update the value of column Supervisor_approval in Reports table
         * with value "REJECTED" and the value of column Accounts_approvale in the table
         * with value "". 
         */
        public bool Reject(int reportId)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.UpdateSupReject(UserId, reportId);
            return true;
        }

        /*
         * This will calulate total expenses in this current month.
         * Expenses approved by both accounts staff and supervisor
         * OR expenses approved by supervisor and not rejected by accounts staff(still processing)
         */
        public void CalculateExpenses()
        {
            DateTime today = DateTime.Today;
            string startOfMonth = new DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd");
            
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
            adapter.FillByDeptExpenses(reportsTable, Department, startOfMonth);

            foreach (Reports.ReportsRow row in reportsTable)
            {
                Expenses += row.Total_amount;
            }
            BudgetRemained = MonthlyBudget - Expenses;
        }

        /*
         * reset all calcullated expenses and budget
         */
        public void ResetBudget()
        {
            Expenses = 0;
            BudgetRemained = 0;
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

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
     * The Class for consultants in Blue Consulting.
     */
    public class Consultant : User
    {
        /*
         * list of expenses in a report
         */
        public List<Expense> Expenses { get; set; }

        public Consultant(string userId) : base(userId)
        {
            Expenses = new List<Expense>();
        }

        /*
         * Consultants display all reports, approved reports and reports that is waiting
         * for approval. This method will return the value as ReportsDataTable.
         * So that this object will be binded with GridView in UI project.
         */
        public override Reports.ReportsDataTable GetReports(int type)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
            
            switch (type)
            {
                //for all reports written by the consultant
                case 1:
                    adapter.FillByUsername(reportsTable, UserId);
                    break;
                //for approved reports written by the consultant
                case 2:
                    adapter.FillByApproval(reportsTable, UserId);
                    break;
                //for reports waiting for approval
                case 3:
                    adapter.FillByNotApproval(reportsTable, UserId);
                    break;
            }
            return reportsTable;
        }

        /*
         * This method will be called when a consultant click submit report button.
         * This will insert values into Reports table in DB.
         * And retreive reportId from the Reports table and call AddExpenses method
         * with reportId to make all expenses in a report have the same reportId
         */
        public void AddReport(string reportName)
        {
            int reportId = 0;
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.Fill(reportsTable);
            adapter.Insert(reportName, 0, "PROCESSING", "PROCESSING", Department, UserId, "NONE", DateTime.Now.Date);
            
            adapter.FillByReportName(reportsTable, reportName);
            foreach (Reports.ReportsRow report in reportsTable)
            {
                reportId = report.Id;
            }
            AddExpenses(reportId);
            
            //After saving all data, reset list of expense
            Expenses = new List<Expense>();
        }

        /*
         * This inserts all expenses in a report into Expenses table.
         * The expenses in the same report will have the same value reportId
         * while inserting values into Expenses table, this will calculate total amount
         * of expenses in a report and call updateReportToTalAmount method will total amount.
         */
        public void AddExpenses(int reportId)
        {
            ExpensesTableAdapter adapter = new ExpensesTableAdapter();
            Reports.ExpensesDataTable expensesTable = new Reports.ExpensesDataTable();
            double totalExpenses = 0;

            adapter.Fill(expensesTable);
            foreach (Expense expense in Expenses)
            {
                adapter.Insert(expense.Date, expense.Location, expense.Description, expense.Amount, expense.Currency, expense.Amount_aud, expense.Receipt, reportId);
                totalExpenses += expense.Amount_aud;
            }
            UpdateReportTotalAmount(reportId, totalExpenses);
        }

        /*
         * This method will be called when a consultant clicks add expense button.
         * this will create an instance of Expense class and add it to the list of expense.
         */
        public void CreateExpense(DateTime date, string location, string description, string strAmount, string currency, byte[] receipt)
        {
            double amount;
            Double.TryParse(strAmount, out amount);
            double amount_aud = ConvertToAUD(currency, amount);
            Expenses.Add(new Expense(date, location, description, Math.Round(amount, 2), currency, amount_aud, receipt));
        }

        /*
         * This will update the value of the colume Total_amount in Reports table
         */
        private void UpdateReportTotalAmount(int reportId, double totalExpenses)
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.UpdateTotalAmount(totalExpenses, reportId);
        }
        
        /*
         * This converts amount in Euros and CNY into Australian dollar
         */
        public double ConvertToAUD(string currency, double amount)
        {
            switch (currency)
            {
                case ("Euros"):
                    amount = amount * 1.48;
                    break;
                case ("CNY"):
                    amount = amount * 0.17;
                    break;
            }
            return Math.Round(amount, 2);
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

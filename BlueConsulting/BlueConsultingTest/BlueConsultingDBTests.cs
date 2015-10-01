using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using System.Transactions;
using System.Xml;
using BlueConsultingDB;
using BlueConsultingDB.ReportsTableAdapters;

namespace BlueConsultingTest
{
    [TestClass]
    public class BlueConsultingDBTests
    {
        [TestInitialize]
        public void Setup()
        {
            string pathToData = "../BlueConsultingDB";
            string dataDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToData));
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

        }

        [TestMethod]
        public void GetDepartment_GivenValidConsultantUserId()
        {
            Consultant consultant = new Consultant("ec16b8aa-e928-4445-8fd3-11d56184a1a1");
            Assert.AreEqual("State Services", consultant.Department);

            consultant = new Consultant("6cb4e17b-f8d9-469a-84a7-424ac788afac");
            Assert.AreEqual("Logistics Services", consultant.Department);

            consultant = new Consultant("4a4de22e-cd4f-4031-b0c1-126c3409dda3");
            Assert.AreEqual("Higher Education Services", consultant.Department);
        }

        [TestMethod]
        public void GetDepartment_GivenValidSupervisorUserId()
        {
            Supervisor supervisor = new Supervisor("c97e72e0-34bf-4c7b-8dcf-f8d4c2e5e660");
            Assert.AreEqual("State Services", supervisor.Department);

            supervisor = new Supervisor("ff53c7ba-170f-47b3-9bd3-1a64287d157c");
            Assert.AreEqual("Logistics Services", supervisor.Department);

            supervisor = new Supervisor("c252413a-1f6c-4086-b35f-1e430f7d7a14");
            Assert.AreEqual("Higher Education Services", supervisor.Department);
        }

        [TestMethod]
        public void AddReport_GivenValidExpense_AddsReportAndExpenseToDB()
        {
            DateTime date = DateTime.Now.Date;
            string reportName = "report";
            string location = "AA";
            string description = "BB";
            string strAmount = "100";
            double amount = Math.Round(100.000, 2);
            string currency = "AUD";
            byte[] receipt = new byte[] { 1, 2, 3, 4 };
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("ce65a8ac-728b-405e-a00d-4a14e6ad23dc");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, location, description, strAmount, currency, receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport(reportName);

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(reportId, report.Id);
                    Assert.AreEqual(reportName, report.Report_name);
                    Assert.AreEqual(amount, report.Total_amount);
                    Assert.AreEqual("PROCESSING", report.Supervisor_approval);
                    Assert.AreEqual("PROCESSING", report.Accounts_approval);
                    Assert.AreEqual(consultant.Department, report.Department);
                    Assert.AreEqual("ce65a8ac-728b-405e-a00d-4a14e6ad23dc", report.Consultant_id);
                    Assert.AreEqual("NONE", report.Supervisor_id);
                }

                ExpensesTableAdapter adapter2 = new ExpensesTableAdapter();
                Reports.ExpensesDataTable expensesTable = new Reports.ExpensesDataTable();
                adapter2.FillByReportId(expensesTable, reportId);

                foreach (Reports.ExpensesRow expense in expensesTable)
                {
                    Assert.AreEqual(date, expense.Date);
                    Assert.AreEqual(location, expense.Location);
                    Assert.AreEqual(description, expense.Description);
                    Assert.AreEqual(amount, expense.Amount);
                    Assert.AreEqual(currency, expense.Currency);
                    Assert.AreEqual(amount, expense.Amount_aud);
                }
                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void AddReport_GivenValidExpense_AddsReportAndExpenseToDB2()
        {
            DateTime date = DateTime.Now.Date;
            string reportName = "REPORT";
            string location = "Broadway";
            string description = "food";
            string strAmount = "100.1111";
            double amount = Math.Round(100.1111, 2);
            double amount_aud = 148.16;
            string currency = "Euros";
            byte[] receipt = new byte[] { 2, 3, 4, 2 };
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("41651a9d-3122-4ea2-bfe0-dca38d7248d1");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, location, description, strAmount, currency, receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport(reportName);

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(reportId, report.Id);
                    Assert.AreEqual(reportName, report.Report_name);
                    Assert.AreEqual(amount, report.Total_amount);
                    Assert.AreEqual("PROCESSING", report.Supervisor_approval);
                    Assert.AreEqual("PROCESSING", report.Accounts_approval);
                    Assert.AreEqual(consultant.Department, report.Department);
                    Assert.AreEqual(consultant.Username, report.Consultant_id);
                    Assert.AreEqual("NONE", report.Supervisor_id);
                }

                ExpensesTableAdapter adapter2 = new ExpensesTableAdapter();
                Reports.ExpensesDataTable expensesTable = new Reports.ExpensesDataTable();
                adapter2.FillByReportId(expensesTable, reportId);

                foreach (Reports.ExpensesRow expense in expensesTable)
                {
                    Assert.AreEqual(date, expense.Date);
                    Assert.AreEqual(location, expense.Location);
                    Assert.AreEqual(description, expense.Description);
                    Assert.AreEqual(amount, expense.Amount);
                    Assert.AreEqual(currency, expense.Currency);
                    Assert.AreEqual(amount_aud, expense.Amount_aud);
                }
                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void AddReport_HavingMultipleExpenses_AddsReportAndExpenseToDB()
        {
            string strAmount = "100.1111";
            double amount_aud = 100.11;
            string strAmount2 = "50.222";
            double amount_aud2 = 74.33;
            string strAmount3 = "99";
            double amount_aud3 = 16.83;
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("6cb4e17b-f8d9-469a-84a7-424ac788afac");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(DateTime.Now, "", "", strAmount, "AUD", null);
                consultant.CreateExpense(DateTime.Now, "", "", strAmount2, "Euros", null);
                consultant.CreateExpense(DateTime.Now, "", "", strAmount3, "CNY", null);
                consultant.AddExpenses(reportId);
                consultant.AddReport("");

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(amount_aud + amount_aud2 + amount_aud3, report.Total_amount);
                }
                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void AddReport_ApprovedBySupAndApprovedByAcc_GetReportStatus()
        {
            DateTime date = DateTime.Now.Date;
            string reportName = "REPORT";
            string location = "Broadway";
            string description = "food";
            string strAmount = "100.1111";
            double amount = Math.Round(100.1111, 2);
            double amount_aud = 148.16;
            string currency = "Euros";
            byte[] receipt = new byte[] { 2, 3, 4, 2 };
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("con_he5");
            Supervisor supervisor = new Supervisor("sup_he1");
            AccountsStaff accountsStaff = new AccountsStaff("acc1");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, location, description, strAmount, currency, receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport(reportName);
                supervisor.Approve(reportId);
                accountsStaff.Approve(reportId);

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(reportId, report.Id);
                    Assert.AreEqual(reportName, report.Report_name);
                    Assert.AreEqual(amount, report.Total_amount);
                    Assert.AreEqual("APPROVED", report.Supervisor_approval);
                    Assert.AreEqual("APPROVED", report.Accounts_approval);
                    Assert.AreEqual(consultant.Department, report.Department);
                    Assert.AreEqual(consultant.Username, report.Consultant_id);
                    Assert.AreEqual("NONE", report.Supervisor_id);
                }

                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void AddReport_ApprovedBySupAndRejectedByAcc_GetReportStatus()
        {
            DateTime date = DateTime.Now.Date;
            string reportName = "REPORT";
            string location = "Department Store";
            string description = "computer";
            string strAmount = "1000.567";
            double amount = Math.Round(1000.567, 2);
            byte[] receipt = new byte[] { 2, 3, 4, 2 };
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("41651a9d-3122-4ea2-bfe0-dca38d7248d1");
            Supervisor supervisor = new Supervisor("c252413a-1f6c-4086-b35f-1e430f7d7a14");
            AccountsStaff accountsStaff = new AccountsStaff("f447b9f3-6733-42aa-b1c5-05a8b4451ca7");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, location, description, strAmount, "AUD", receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport(reportName);
                supervisor.Approve(reportId);
                accountsStaff.Reject(reportId);

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(reportId, report.Id);
                    Assert.AreEqual(reportName, report.Report_name);
                    Assert.AreEqual(amount, report.Total_amount);
                    Assert.AreEqual("APPROVED", report.Supervisor_approval);
                    Assert.AreEqual("REJECTED", report.Accounts_approval);
                    Assert.AreEqual(consultant.Department, report.Department);
                    Assert.AreEqual(consultant.Username, report.Consultant_id);
                    Assert.AreEqual("NONE", report.Supervisor_id);
                }

                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void AddReport_RejectedBySup_GetReportStatus()
        {
            DateTime date = DateTime.Now.Date;
            string reportName = "REPORT";
            string location = "foodcourt";
            string description = "food";
            string strAmount = "1000.567";
            double amount = Math.Round(1000.567, 2);
            byte[] receipt = new byte[] { 2, 3, 4, 2 };
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("41651a9d-3122-4ea2-bfe0-dca38d7248d1");
            Supervisor supervisor = new Supervisor("c252413a-1f6c-4086-b35f-1e430f7d7a14");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, location, description, strAmount, "AUD", receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport(reportName);
                supervisor.Reject(reportId);

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter.FillById(reportsTable, reportId);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual(reportId, report.Id);
                    Assert.AreEqual(reportName, report.Report_name);
                    Assert.AreEqual(amount, report.Total_amount);
                    Assert.AreEqual("REJECTED", report.Supervisor_approval);
                    Assert.AreEqual("", report.Accounts_approval);
                    Assert.AreEqual(consultant.Department, report.Department);
                    Assert.AreEqual(consultant.Username, report.Consultant_id);
                    Assert.AreEqual("NONE", report.Supervisor_id);
                }

                testTransaction.Dispose(); // rollback
            }
        }

        /* CONNECTION ERROR
         * 
        [TestMethod]
        public void GetPdf_GivenExpenseId_GetByteArray()
        {
            DateTime date = DateTime.Now.Date;
            byte[] receipt = new byte[] { 2, 3, 4, 2, 5, 6, 4, 8, 9};
            int reportId = GetCurrentRowOfReportsTable() + 1;
            int expenseId = GetCurrentRowOfExpensesTable() + 1;
            Consultant consultant = new Consultant("41651a9d-3122-4ea2-bfe0-dca38d7248d1");
            
            using (TransactionScope testTransaction = new TransactionScope())
            {
                consultant.CreateExpense(date, "", "", "", "AUD", receipt);
                consultant.AddExpenses(reportId);
                consultant.AddReport("");
                
                byte[] expected = consultant.GetPdf(expenseId);
                Assert.AreEqual(receipt, expected);
                testTransaction.Dispose();
            }
        }
        */

        [TestMethod]
        public void ConvertToAUD_GetConvertedAmount()
        {
            Consultant consultant = new Consultant("9d63d07e-ca97-43fe-87c2-19b6a8d64979");
            Assert.AreEqual(150.68, consultant.ConvertToAUD("AUD", 150.6789));

            Assert.AreEqual(223.00, consultant.ConvertToAUD("Euros", 150.6789));

            Assert.AreEqual(25.62, consultant.ConvertToAUD("CNY", 150.6789)); Assert.AreEqual(150.68, consultant.ConvertToAUD("AUD", 150.6789));
        }

        [TestMethod]
        public void GetReports_SubmitedBySeveralUserInSameDepartment_DisplayedBySupervisor()
        {
            Consultant con1 = new Consultant("715b8401-6be0-4a63-9376-267909e79200");
            Consultant con2 = new Consultant("089e814b-2315-4cfc-9316-85d2a495bd96");
            Consultant con3 = new Consultant("6cb4e17b-f8d9-469a-84a7-424ac788afac");
            Supervisor sup = new Supervisor("ff53c7ba-170f-47b3-9bd3-1a64287d157c");
            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                con1.CreateExpense(DateTime.Now, "", "", "", "", null);
                con1.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con1.AddReport("con1");

                con2.CreateExpense(DateTime.Now, "", "", "", "", null);
                con2.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con2.AddReport("con2");

                con3.CreateExpense(DateTime.Now, "", "", "", "", null);
                con3.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con3.AddReport("con3");

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = sup.GetReports(0);

                int counter = 1;
                foreach (Reports.ReportsRow report in reportsTable)
                {
                    if (counter == 1)
                    {
                        Assert.AreEqual("con1", report.Report_name);
                        Assert.AreEqual("715b8401-6be0-4a63-9376-267909e79200", report.Consultant_id);
                    }
                    else if (counter == 2)
                    {
                        Assert.AreEqual("con2", report.Report_name);
                        Assert.AreEqual("089e814b-2315-4cfc-9316-85d2a495bd96", report.Consultant_id);
                    }
                    else
                    {
                        Assert.AreEqual("con3", report.Report_name);
                        Assert.AreEqual("6cb4e17b-f8d9-469a-84a7-424ac788afac", report.Consultant_id);
                    }
                    counter++;
                }
                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void GetReports_SubmitedByConsultantInDifferentDepartment_DisplayedBySupervisorInSameDepartment()
        {
            Consultant con_s1 = new Consultant("ce65a8ac-728b-405e-a00d-4a14e6ad23dc");
            Consultant con_l1 = new Consultant("715b8401-6be0-4a63-9376-267909e79200");
            Consultant con_he1 = new Consultant("4a4de22e-cd4f-4031-b0c1-126c3409dda3");
            Supervisor sup_s1 = new Supervisor("1b52538c-ee54-425b-a3bc-e1b5cab1fdd3");
            Supervisor sup_l1 = new Supervisor("ff53c7ba-170f-47b3-9bd3-1a64287d157c");
            Supervisor sup_he1 = new Supervisor("c252413a-1f6c-4086-b35f-1e430f7d7a14");

            // start transaction
            using (TransactionScope testTransaction = new TransactionScope())
            {
                con_s1.CreateExpense(DateTime.Now, "", "", "", "", null);
                con_s1.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con_s1.AddReport("con_s1");

                con_l1.CreateExpense(DateTime.Now, "", "", "", "", null);
                con_l1.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con_l1.AddReport("con_l1");

                con_he1.CreateExpense(DateTime.Now, "", "", "", "", null);
                con_he1.AddExpenses(GetCurrentRowOfExpensesTable() + 1);
                con_he1.AddReport("con_he1");

                ReportsTableAdapter adapter = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = sup_s1.GetReports(0);

                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual("con_s1", report.Report_name);
                    Assert.AreEqual("ce65a8ac-728b-405e-a00d-4a14e6ad23dc", report.Consultant_id);
                }

                adapter = new ReportsTableAdapter();
                reportsTable = sup_l1.GetReports(0);
                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual("con_l1", report.Report_name);
                    Assert.AreEqual("715b8401-6be0-4a63-9376-267909e79200", report.Consultant_id);
                }

                adapter = new ReportsTableAdapter();
                reportsTable = sup_he1.GetReports(0);
                foreach (Reports.ReportsRow report in reportsTable)
                {
                    Assert.AreEqual("con_he1", report.Report_name);
                    Assert.AreEqual("4a4de22e-cd4f-4031-b0c1-126c3409dda3", report.Consultant_id);
                }

                testTransaction.Dispose(); // rollback
            }
        }


        [TestMethod]
        public void CalcaluateExpences_ExpensesNotChanged()
        {
            int reportId = GetCurrentRowOfReportsTable() + 1;

            Consultant con_s1 = new Consultant("ce65a8ac-728b-405e-a00d-4a14e6ad23dc");
            Supervisor sup_s1 = new Supervisor("1b52538c-ee54-425b-a3bc-e1b5cab1fdd3");
            AccountsStaff acc1 = new AccountsStaff("f447b9f3-6733-42aa-b1c5-05a8b4451ca7");
            sup_s1.CalculateExpenses();
            double currentExpenses = sup_s1.Expenses;
            double currentBudgetRemained = sup_s1.BudgetRemained;
            sup_s1.ResetBudget();

            using (TransactionScope testTransaction = new TransactionScope())
            {
                con_s1.CreateExpense(DateTime.Now.Date, "sd", "first", "1000", "AUD", null);
                con_s1.AddReport("new report");
                con_s1.AddExpenses(reportId);

                ReportsTableAdapter adapter2 = new ReportsTableAdapter();
                Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();
                adapter2.FillById(reportsTable, reportId);

                sup_s1.CalculateExpenses();
                Assert.AreEqual(currentExpenses, sup_s1.Expenses);
                sup_s1.ResetBudget();

                testTransaction.Dispose(); // rollback
            }
        }

        private int GetCurrentRowOfReportsTable()
        {
            ReportsTableAdapter adapter = new ReportsTableAdapter();
            Reports.ReportsDataTable reportsTable = new Reports.ReportsDataTable();

            adapter.Fill(reportsTable);

            int lastId = 0;

            foreach (Reports.ReportsRow report in reportsTable)
            {
                lastId = report.Id;
            }
            return lastId;
        }

        private int GetCurrentRowOfExpensesTable()
        {
            ExpensesTableAdapter adapter = new ExpensesTableAdapter();
            Reports.ExpensesDataTable expensesTable = new Reports.ExpensesDataTable();

            adapter.Fill(expensesTable);

            int lastId = 0;

            foreach (Reports.ExpensesRow expense in expensesTable)
            {
                lastId = expense.Id;
            }
            return lastId;
        }
    }
}

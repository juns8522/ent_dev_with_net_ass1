using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueConsultingDB
{
    /*
     * This class is for expenses in a report.
     * Consultant will possess the List of this instance.
     * After consultant clicks submit report button,
     * the values in the instance of this class will be saved in DB.
     */
    public class Expense
    {
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Amount_aud { get; set; }
        public string Currency { get; set; }
        public byte[] Receipt { get; set; }
        public int ReportId { get; set; }

        public Expense(DateTime date, string location, string description, double amount, string currency, double amount_aud, byte[] receipt)
        {
            Date = date;
            Location = location;
            Description = description;
            Amount = amount;
            Currency = currency;
            Amount_aud = amount_aud;
            Receipt = receipt;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Emerge.Models
{
    public class Accounts
    {
        public int id { get; set; }
        public string empCode { get; set; } 
        public string empName { get; set; }
        public string gender { get; set; }
        public string department { get; set; }
        public string payPeriod { get; set; }
        public DateTime doj { get; set; } 
        public string position { get; set; }
        public string bankName { get; set; }
        public int accNo { get; set; } 
        public string payMode { get; set; }
        public string basic { get; set; }
        public string hra { get; set; }
        public string medAllowance { get; set; }
        public string conAllowance { get; set; }
        public string flexiAllowance { get; set; }
        public string providentFund { get; set; }
        public string tds { get; set; }
        public string profTax { get; set; }
        public string lop { get; set; }
        public string netPay { get; set; }
        public string month { get; set; }
        public int year { get; set; }
    }
}

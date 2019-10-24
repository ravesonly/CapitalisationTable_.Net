using System;
using System.Collections.Generic;
using System.Text;

namespace CapTableByDate
{
    class Equity
    {
        public DateTime InvestmentDate { get; set; }
        public int SharesPurchased { get; set; }
        public  double CashPaid { get; set; }
        public string Investor { get; set; }
    }
}

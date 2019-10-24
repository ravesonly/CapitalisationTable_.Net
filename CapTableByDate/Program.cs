//This program gives output as requested.
//I believe this code can be extensible to run and get capitalization at certain date. We need to pass date as second argument and then filter records before that date.


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CapTableByDate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Check for arguments
            if (args != null && args.Length >0 && !string.IsNullOrEmpty(args[0]))
            {
                var path = args[0];
                if (File.Exists(path))
                {
                    try
                    {
                        //read csv file into list
                        var lines = File.ReadAllLines(path).Skip(1);
                        List<Equity> equities = new List<Equity>();
                        Equity equity;
                        foreach (var line in lines)
                        {
                            equity = new Equity();
                            string[] words = line.Split(',');
                            equity.InvestmentDate = Convert.ToDateTime(words[0].Trim());
                            equity.SharesPurchased = Convert.ToInt32(words[1].Trim());
                            equity.CashPaid = Convert.ToDouble(words[2].Trim());
                            equity.Investor = words[3].Trim();
                            equities.Add(equity);
                        }
                        //Get data by investors
                        var ownership = equities.GroupBy(e => e.Investor).Select(equity => new
                        {
                            investor = equity.Key,
                            shares = equity.Sum(x => x.SharesPurchased),
                            cash_paid = equity.Sum(x => x.CashPaid),
                            ownership = String.Format("{0:0.00}", (double)equity.Sum(x => x.SharesPurchased) * 100 / equities.Sum(e => e.SharesPurchased))
                        });
                        var f = new FinalResult();
                        f.date = equities.OrderByDescending(e => e.InvestmentDate).First().InvestmentDate.ToString("MM/dd/yyyy");
                        f.cash_raised = equities.Sum(e => e.CashPaid);
                        f.total_number_of_shares = equities.Sum(e => e.SharesPurchased);
                        f.ownership = ownership;
                        Console.WriteLine(JsonConvert.SerializeObject(f));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Some error occured. Details:" + ex.Message);
                    }
                }
                else
                    Console.WriteLine("File don't exist on given path.");
            }
            else
                Console.WriteLine("Please enter valid path.");
        }
    }
}

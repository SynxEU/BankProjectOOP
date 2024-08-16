using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public class AccountModel
    {
        public int AccountNumber { get; set; }
        public int TitleID { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }
    }
}

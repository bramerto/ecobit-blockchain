using System.Collections.Generic;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class UserHistory
    {
        public string User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
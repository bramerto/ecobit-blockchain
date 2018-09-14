using System.Collections.Generic;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class SupplyChain
    {
        public Transaction Transaction { get; set; }

        public List<SupplyChain> Children { get; set; }

        public SupplyChain(Transaction transaction)
        {
            Transaction = transaction;
            Children = new List<SupplyChain>();
        }

        public void AddChild(SupplyChain chain)
        {
            Children.Add(chain);
        }

       
    }
}
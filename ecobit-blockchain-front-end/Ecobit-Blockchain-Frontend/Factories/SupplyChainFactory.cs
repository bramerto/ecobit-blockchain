using System.Collections.Generic;
using System.Linq;
using Ecobit_Blockchain_Frontend.Models;

namespace Ecobit_Blockchain_Frontend.Factories
{
    public static class SupplyChainFactory
    {
        public static SupplyChain Make(List<Transaction> transactions)
        {
            transactions = transactions.OrderBy(t => t.OrderTime).ToList();

            SupplyChain supplyChain = null;
            
            foreach (var transaction in transactions)
            {
                if (supplyChain == null)
                {
                    supplyChain = new SupplyChain(transaction);
                }
                else
                {
                    AddTransactionToSupplyChain(supplyChain, transaction);
                }
            }

            return supplyChain;
        }

        private static void AddTransactionToSupplyChain(SupplyChain chain, Transaction transaction)
        {
            if (chain.Transaction.ToOwner == transaction.FromOwner)
            {
                var childChain = new SupplyChain(transaction);
                chain.AddChild(childChain);
            }
            else
            {
                foreach (var childChain in chain.Children)
                {
                    AddTransactionToSupplyChain(childChain, transaction);
                }
            }
        }
    }
}
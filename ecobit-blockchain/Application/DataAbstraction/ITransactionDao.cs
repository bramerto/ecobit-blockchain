using Application.Models;

namespace Application.DataAbstraction
{
    public interface ITransactionDao
    {
        /// <summary>
        /// Save the transaction in a datastore
        /// </summary>
        /// <param name="transaction"></param>
        void SaveTransaction(Transaction transaction);
        
    }
}
using System;
using System.Collections.Generic;
using Ecobit_Blockchain_Frontend.Models;

namespace Ecobit_Blockchain_Frontend.DataAccess.Interfaces
{
    public interface ITransactionDao
    {

        /// <summary>
        /// Get all the transactions from a specific batch.
        /// </summary>
        /// <param name="batchId">The id of the batch</param>
        /// <returns>A list of the transactions</returns>
        List<Transaction> GetTransactionsByBatchId(int batchId);

        /// <summary>
        /// Get all the transactions of a specific user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A list of the transactions</returns>
        List<Transaction> GetTransactionsByUser(String user);

        /// <summary>
        /// Saves a transaction to the blockchain
        /// </summary>
        /// <param name="transaction"></param>
        void SaveTransaction(Transaction transaction);
    }
}
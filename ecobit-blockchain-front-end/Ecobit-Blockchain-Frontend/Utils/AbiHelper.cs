using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Ecobit_Blockchain_Frontend.Utils
{
    public static class AbiHelper
    {
        /// <summary>
        /// Get the abi of the TransactionManager contract.
        /// </summary>
        /// <returns>The abi in string format</returns>
        public static string GetEcobitBlockchainAbi()
        {
            return GetAbi("/truffle/build/contracts/EcobitBlockchain.json");
        }

        /// <summary>
        /// Get the abi of the Transaction contract.
        /// </summary>
        /// <returns>The abi in string format</returns>
        public static string GetTransactionAbi()
        {
            return GetAbi("/truffle/build/contracts/Transaction.json");
        }

        private static string GetAbi(string filePath)
        {
            var contractJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + filePath);

            var contractObject = JObject.Parse(contractJson);
            
            return Newtonsoft.Json.JsonConvert.SerializeObject(contractObject["abi"]);
        }
    }
}
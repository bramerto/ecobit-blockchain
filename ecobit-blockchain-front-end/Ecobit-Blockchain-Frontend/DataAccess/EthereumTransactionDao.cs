using Ecobit_Blockchain_Frontend.Models;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Utils;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Ecobit_Blockchain_Frontend.DataAccess
{
    public class EthereumTransactionDao : ITransactionDao
    {
        public dynamic Contract { get; set; }
        
        public dynamic TransactionContract { get; set; }
        
        private readonly Web3 _webNode;

        public EthereumTransactionDao()
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["ethUrl"];

            _webNode = url == null ? new Web3() : new Web3(url);
            
            _webNode.TransactionManager.DefaultGasPrice = new BigInteger(0);
            Contract = _webNode.Eth.GetContract(AbiHelper.GetEcobitBlockchainAbi(), System.Configuration.ConfigurationManager.AppSettings["transactionManagerAddress"]);
        }

        public List<Transaction> GetTransactionsByBatchId(int batchId)
        {
            List<string> addresses = GetTransactionAddresses("getTransactions", batchId);
            return AddressToTransactions(addresses);
        }

        public List<Transaction> GetTransactionsByUser(string user)
        {
            List<string> addresses = GetTransactionAddresses("getUserTransactions", user);
            return AddressToTransactions(addresses);
        }

        private List<Transaction> AddressToTransactions(List<string> addresses)
        {
            List<Transaction> transactions = new List<Transaction>();

            foreach (var address in addresses)
            {
                transactions.Add(GetTransactionByAddress(address));
            }

            return transactions;
        }

        private List<string> GetTransactionAddresses(string functionName, int tag)
        {
            dynamic function = Contract.GetFunction(functionName);

            Task<List<string>> task = function.CallAsync<List<string>>(tag);

            List<string> addresses = task.ContinueWith(response =>
            {
                return response.Result;
            }).Result;
            task.Wait();

            return addresses;
        }
        
        private List<string> GetTransactionAddresses(string functionName, string tag)
        {
            dynamic function = Contract.GetFunction(functionName);

            Task<List<string>> task = function.CallAsync<List<string>>(tag);

            List<string> addresses = task.ContinueWith(response => response.Result).Result;
            task.Wait();

            return addresses;
        }

        private Transaction GetTransactionByAddress(string address)
        {
            int batchId = CallTransactionFunction<int>(address, "getBatchID");
            string transactionId = CallTransactionFunction<string>(address, "getTransactionID");
            int quantity = CallTransactionFunction<int>(address, "getQuantity");
            int itemPrice = CallTransactionFunction<int>(address, "getItemPrice");
            int orderDate = CallTransactionFunction<int>(address, "getOrderDate");
            string ownerFrom = CallTransactionFunction<string>(address, "getFromOwner");
            string ownerTo = CallTransactionFunction<string>(address, "getToOwner");

            string transporter = CallTransactionFunction<string>(address, "getTransporter");
            int pickupDate = CallTransactionFunction<int>(address, "getTransportPickupDate");
            int deliverDate = CallTransactionFunction<int>(address, "getTransportDeliverDate");
            
            Transport transport = new Transport
            {
                Transporter = transporter, 
                PickupDate = DateTimeUtil.ConvertToDate(pickupDate), 
                DeliverDate = DateTimeUtil.ConvertToDate(deliverDate)
            };
            
            return new Transaction
            {
                BatchId = batchId,
                TransactionId = transactionId,
                Quantity = quantity,
                ItemPrice = itemPrice,
                OrderTime = DateTimeUtil.ConvertToDate(orderDate),
                FromOwner = ownerFrom,
                ToOwner = ownerTo,
                Transport = transport
            };
        }

        private T CallTransactionFunction<T>(string address, string functionName)
        {
            dynamic transactionContract;
            
            if (TransactionContract == null)
            {
                transactionContract = _webNode.Eth.GetContract(AbiHelper.GetTransactionAbi(), address);
            }
            else
            {
                transactionContract = TransactionContract;
            }

            return CallTransactionFunction<T>(functionName, transactionContract);
        }

        private T CallTransactionFunction<T>(string functionName, dynamic transactionContract)
        {
            dynamic function = transactionContract.GetFunction(functionName);
            
            Task<T> task = function.CallAsync<T>();

            T result = task.ContinueWith(response => response.Result).Result;
            
            task.Wait();
            return result;
        }

        
        public void SaveTransaction(Transaction transaction)
        {
            if (transaction.Transport != null) return;
            dynamic function = Contract.GetFunction("addTransaction");

            transaction.TransactionId = GenerateIdUtil.GenerateUniqueId();
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(4712388),
                new HexBigInteger(0),
                transaction.BatchId,
                transaction.TransactionId,
                transaction.Quantity,
                transaction.ItemPrice,
                DateTimeUtil.ConvertToTimestamp(transaction.OrderTime),
                transaction.FromOwner,
                transaction.ToOwner);
            
            task.ContinueWith(response => response.Result);

            task.Wait();
        }
    }
}
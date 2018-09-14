using System;
using Application.DataAbstraction.NethereumAbstraction;
using Application.Util;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using ProxyFoo;
using Transaction = Application.Models.Transaction;

namespace Application.DataAbstraction
{
    public class EthereumTransactionDao : ITransactionDao
    {
        public IContract Contract { get; set; }

        private readonly ILogger _logger;

        public EthereumTransactionDao() : this(DependencyFactory.Resolve<ILogger>())
        {
        }

        public EthereumTransactionDao(ILogger logger)
        {
            _logger = logger;

            var webNode = new Web3(System.Configuration.ConfigurationManager.AppSettings["ethUrl"])
            {
                TransactionManager = {DefaultGasPrice = 0}
            };

            Contract = Duck.Cast<IContract>(webNode.Eth.GetContract(AbiHelper.GetTransactionManagerAbi(), System.Configuration.ConfigurationManager.AppSettings["contractAddress"]));
        }

        /// <summary>
        /// Saves a Transaction object to the smartcontract.
        /// </summary>
        public void SaveTransaction(Transaction transaction)
        {
            if (transaction.Transport != null)
            {
                SaveTransactionWithTransport(transaction);
            }
            else
            {
                var function = Duck.Cast<IFunction>(Contract.GetFunction("addTransaction"));
                
                var task = function.SendTransactionAsync(
                    System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                    new HexBigInteger(4712388),
                    new HexBigInteger(0),
                    transaction.BatchId,
                    transaction.TransactionId,
                    transaction.Quantity,
                    Convert.ToInt32(transaction.ItemPrice * 100.00),
                    DateTimeUtil.ConvertToTimestamp(transaction.OrderTime),
                    transaction.From,
                    transaction.To);

                task.ContinueWith(response =>
                {
                    _logger.Log(response.Result);

                    return response.Result;
                });

                task.Wait();
            }
        }

        /// <summary>
        /// Saves a Transaction object with transport to the smartcontract.
        /// </summary>
        private void SaveTransactionWithTransport(Transaction transaction)
        {
            var function = Duck.Cast<IFunction>(Contract.GetFunction("addTransactionWithTransport"));
            
            var task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(4712388), 
                new HexBigInteger(0),
                transaction.BatchId,
                transaction.TransactionId,
                transaction.Quantity,
                Convert.ToInt32(transaction.ItemPrice * 100.00),
                DateTimeUtil.ConvertToTimestamp(transaction.OrderTime),
                transaction.From,
                transaction.To,
                transaction.Transport.Transporter,
                DateTimeUtil.ConvertToTimestamp(transaction.Transport.PickupDate),
                DateTimeUtil.ConvertToTimestamp(transaction.Transport.DeliverDate));

            task.ContinueWith(response =>
            {
                _logger.Log(response.Result);

                return response.Result;
            });

            task.Wait();
        }

    }
}
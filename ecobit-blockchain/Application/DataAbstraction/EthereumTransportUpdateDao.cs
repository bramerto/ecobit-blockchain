using Application.DataAbstraction.NethereumAbstraction;
using Application.Models;
using Application.Util;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using ProxyFoo;

namespace Application.DataAbstraction
{
    public class EthereumTransportUpdateDao : ITransportUpdateDao
    {
        public IContract Contract { get; set; }
        private readonly ILogger _logger;

        public EthereumTransportUpdateDao() : this(DependencyFactory.Resolve<ILogger>())
        {
        }

        public EthereumTransportUpdateDao(ILogger logger)
        {
            _logger = logger;

            var webNode = new Web3(System.Configuration.ConfigurationManager.AppSettings["ethUrl"])
            {
                TransactionManager = {DefaultGasPrice = 0}
            };

            Contract = Duck.Cast<IContract>(webNode.Eth.GetContract(AbiHelper.GetTransactionManagerAbi(),
                System.Configuration.ConfigurationManager.AppSettings["contractAddress"]));
        }

        /// <inheritdoc />
        public void ExecuteTransportUpdate(TransportUpdate update)
        {
            var function = Duck.Cast<IFunction>(Contract.GetFunction("updateTransport"));

            var transporter = update.Transport.Transporter ?? "";
            var pickupDate = DateTimeUtil.ConvertToTimestamp(update.Transport.PickupDate);
            var deliverDate = DateTimeUtil.ConvertToTimestamp(update.Transport.DeliverDate);

            var task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(4712388),
                new HexBigInteger(0),
                update.TransactionId,
                transporter,
                pickupDate,
                deliverDate);

            task.ContinueWith(response =>
            {
                _logger.Log("Update executed: " + response.Result);
                return response;
            });

            task.Wait();
        }
    }
}
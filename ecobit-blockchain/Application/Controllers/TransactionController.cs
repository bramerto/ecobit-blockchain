using Application.DataAbstraction;
using Application.Util;

namespace Application.Controllers
{
    public class TransactionController
    {
        private readonly ITransactionDao _transactionDao;
        private readonly ITransportUpdateDao _transportUpdateDao;
        private IParser Parser;
        
        public TransactionController(): this(
            DependencyFactory.Resolve<ITransactionDao>(),
            DependencyFactory.Resolve<ITransportUpdateDao>(),
            DependencyFactory.Resolve<IParser>())
        {
        }

        public TransactionController(ITransactionDao transactionDao, ITransportUpdateDao transportUpdateDao, IParser parser)
        {
            _transportUpdateDao = transportUpdateDao;
            _transactionDao = transactionDao;
            Parser = parser;
        }

        /// <summary>
        /// Save the transaction within the xml to a datastore
        /// </summary>
        /// <param name="xml"></param>
        public void SaveTransactionXml(string xml)
        {
            var transactions = Parser.ParseTransactions(xml);
            var transportUpdates = Parser.ParseTransportUpdates(xml);

            foreach (var transaction in transactions)
            {
                _transactionDao.SaveTransaction(transaction);
            }

            foreach (var update in transportUpdates)
            {
                _transportUpdateDao.ExecuteTransportUpdate(update);
            }
        }
        
    }
}
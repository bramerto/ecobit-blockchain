using System.Collections.Generic;
using Application.Controllers;
using Application.DataAbstraction;
using Application.Models;
using Application.Util;
using Moq;
using NUnit.Framework;

namespace Application.Tests.Controllers
{
    public class TransactionControllerTest
    {
        [Test]
        public void ThatTransactionIsSavedAndParsed()
        {
            Mock<IParser> parser = new Mock<IParser>();            
            Mock<ITransactionDao> dao = new Mock<ITransactionDao>();
            Mock<ITransportUpdateDao> transportDao = new Mock<ITransportUpdateDao>();
            
            var controller = new TransactionController(dao.Object, transportDao.Object, parser.Object);
            
            var transactions = new List<Transaction>();
            var transaction = new Transaction();
            transactions.Add(transaction);
            parser.Setup(mock => mock.ParseTransactions("test transaction")).Returns(transactions);

            var updates = new List<TransportUpdate>();
            parser.Setup(mock => mock.ParseTransportUpdates(It.IsAny<string>())).Returns(updates);
    
            controller.SaveTransactionXml("test transaction");
            
            parser.Verify(mock => mock.ParseTransactions("test transaction"));
            dao.Verify(mock => mock.SaveTransaction(transaction));
            
        }
    }
}
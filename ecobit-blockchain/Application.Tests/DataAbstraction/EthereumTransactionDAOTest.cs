using System;
using Application.DataAbstraction;
using Application.DataAbstraction.NethereumAbstraction;
using Application.Models;
using Moq;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;

namespace Application.Tests.DataAbstraction
{
    public class EthereumTransactionDaoTest
    {
        private EthereumTransactionDao _dao;

        private Mock<IContract> _contractMock;
        private Mock<IFunction> _functionMock;

        [SetUp]
        public void Init()
        {
            _dao = new EthereumTransactionDao();

            _contractMock = new Mock<IContract>();
            _dao.Contract = _contractMock.Object;

            _functionMock = new Mock<IFunction>();
            _contractMock.Setup(m => m.GetFunction(It.IsAny<String>())).Returns(_functionMock.Object);
        }

        [Test]
        public void ThatTransactionIsSaved()
        {
            Transaction t = new Transaction();
            t.BatchId = 1;
            t.TransactionId = "62FA647C-AD54-4BCC-A860-E5A2664B019D";
            t.Quantity = 2;
            t.ItemPrice = 200.12;
            t.OrderTime = new DateTime(2018, 10, 1);
            t.From = "From company";
            t.To = "To company";

            _dao.SaveTransaction(t);
            
            _contractMock.Verify(((m => m.GetFunction("addTransaction"))));
            _functionMock.Verify((m => m.SendTransactionAsync(It.IsAny<String>(), It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(), 1, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 2, 20012, 1538352000, "From company", "To company")));
        }

        [Test]
        public void ThatTransactionWithTransportIsSaved()
        {
            Transaction t = new Transaction();
            t.BatchId = 1;
            t.TransactionId = "62FA647C-AD54-4BCC-A860-E5A2664B019D";
            t.Quantity = 2;
            t.ItemPrice = 200.12;
            t.OrderTime = new DateTime(2018, 10, 1);
            t.From = "From company";
            t.To = "To company";

            t.Transport = new Transport();
            t.Transport.Transporter = "transport company";
            t.Transport.DeliverDate = new DateTime(2018, 10, 2);
            t.Transport.PickupDate = new DateTime(2018, 10, 3);

            _dao.SaveTransaction(t);

            _contractMock.Verify(((m => m.GetFunction("addTransactionWithTransport"))));
            _functionMock.Verify((m => m.SendTransactionAsync(It.IsAny<String>(), It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(), 1, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 2, 20012, 1538352000, "From company", "To company",
                "transport company", 1538524800, 1538438400)));
        }

      
    }
}
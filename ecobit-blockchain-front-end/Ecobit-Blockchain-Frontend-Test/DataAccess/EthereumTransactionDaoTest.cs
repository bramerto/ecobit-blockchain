using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecobit_Blockchain_Frontend.DataAccess;
using Ecobit_Blockchain_Frontend.DataAccess.NethereumAbstraction;
using Ecobit_Blockchain_Frontend.Models;
using Moq;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.DataAccess
{
    [TestFixture]
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
            _dao.TransactionContract = _contractMock.Object;

            _functionMock = new Mock<IFunction>();  
            
            var addresses = new List<string> {"address1"};
            
            _contractMock.Setup(m => m.GetFunction("getTransactions")).Returns(_functionMock.Object);
            _contractMock.Setup(m => m.GetFunction("getUserTransactions")).Returns(_functionMock.Object);
            _contractMock.Setup(m => m.GetFunction("addTransaction")).Returns(_functionMock.Object);

            _functionMock.Setup(m => m.CallAsync<List<string>>(1234)).Returns(Task.FromResult(addresses));
            _functionMock.Setup(m => m.CallAsync<List<string>>("username")).Returns(Task.FromResult(addresses));
            
            var getBatchIdFunctionMock = new Mock<IFunction>();
            getBatchIdFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(3241));
            _contractMock.Setup(m => m.GetFunction("getBatchID"))
                .Returns(getBatchIdFunctionMock.Object);
            
            var getTransactionIdFunctionMock = new Mock<IFunction>();
            getTransactionIdFunctionMock.Setup(m => m.CallAsync<string>()).Returns(Task.FromResult("uuid"));
            _contractMock.Setup(m => m.GetFunction("getTransactionID"))
                .Returns(getTransactionIdFunctionMock.Object);
            
            var getQuantityFunctionMock = new Mock<IFunction>();
            getQuantityFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(2));
            _contractMock.Setup(m => m.GetFunction("getQuantity"))
                .Returns(getQuantityFunctionMock.Object);

            var getItemPriceFunctionMock = new Mock<IFunction>();
            getItemPriceFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(1));
            _contractMock.Setup(m => m.GetFunction("getItemPrice"))
                .Returns(getItemPriceFunctionMock.Object);

            var getOrderDateFunctionMock = new Mock<IFunction>();
            getOrderDateFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(800000000));
            _contractMock.Setup(m => m.GetFunction("getOrderDate"))
                .Returns(getOrderDateFunctionMock.Object);
            
            var getFromFunctionMock = new Mock<IFunction>();
            getFromFunctionMock.Setup(m => m.CallAsync<string>()).Returns(Task.FromResult("From"));
            _contractMock.Setup(m => m.GetFunction("getFromOwner"))
                .Returns(getFromFunctionMock.Object);

            var getToFunctionMock = new Mock<IFunction>();
            getToFunctionMock.Setup(m => m.CallAsync<string>()).Returns(Task.FromResult("To"));
            _contractMock.Setup(m => m.GetFunction("getToOwner"))
                .Returns(getToFunctionMock.Object);

            var getTransporterFunctionMock = new Mock<IFunction>();
            getTransporterFunctionMock.Setup(m => m.CallAsync<string>()).Returns(Task.FromResult("Transporter"));
            _contractMock.Setup(m => m.GetFunction("getTransporter"))
                .Returns(getTransporterFunctionMock.Object);
            
            var getTransportPickupDateFunctionMock = new Mock<IFunction>();
            getTransportPickupDateFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(1000000000));
            _contractMock.Setup(m => m.GetFunction("getTransportPickupDate"))
                .Returns(getTransportPickupDateFunctionMock.Object);
            
            
            var getTransportDeliverDateFunctionMock = new Mock<IFunction>();
            getTransportDeliverDateFunctionMock.Setup(m => m.CallAsync<int>()).Returns(Task.FromResult(1200000000));
            _contractMock.Setup(m => m.GetFunction("getTransportDeliverDate"))
                .Returns(getTransportDeliverDateFunctionMock.Object);
        }
        
        [Test]
        public void TestThatTransactionsByBatchIdsAreReturned()
        {
            var result = _dao.GetTransactionsByBatchId(1234);
            var transaction = result[0];

            VerifyTransaction(transaction);
        }

        [Test]
        public void ThatTransactionsByBatchUserAreReturned()
        {
            var result = _dao.GetTransactionsByUser("username");
            var transaction = result[0];

            VerifyTransaction(transaction);
        }


        private void VerifyTransaction(Transaction transaction)
        {
            Assert.AreEqual(3241, transaction.BatchId);
            Assert.AreEqual("uuid", transaction.TransactionId);
            Assert.AreEqual(2, transaction.Quantity);
            Assert.AreEqual(1, transaction.ItemPrice);
            Assert.AreEqual(1995, transaction.OrderTime.Year);

            Assert.AreEqual("From", transaction.FromOwner);
            Assert.AreEqual("To", transaction.ToOwner);
            Assert.AreEqual("Transporter", transaction.Transport.Transporter);
            Assert.AreEqual(2001, transaction.Transport.PickupDate.Year);
            Assert.AreEqual(2008, transaction.Transport.DeliverDate.Year);
        }
        
        [Test]
        public void ThatTransactionIsSaved()
        {
            var t = new Transaction
            {
                BatchId = 1,
                TransactionId = "62FA647C-AD54-4BCC-A860-E5A2664B019D",
                Quantity = 2,
                ItemPrice = 200,
                OrderTime = new DateTime(2018, 10, 1),
                FromOwner = "From company",
                ToOwner = "To company"
            };
            
            _dao.SaveTransaction(t);

            _contractMock.Verify(m => m.GetFunction("addTransaction"));
            _functionMock.Verify(m => m.SendTransactionAsync(
                It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                1,
                It.IsAny<string>(),
                2,
                200,
                1538352000,
                "From company",
                "To company"
            ));
        }
    }
}
using System;
using Application.DataAbstraction;
using Application.DataAbstraction.NethereumAbstraction;
using Application.Models;
using Moq;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;

namespace Application.Tests.DataAbstraction
{
    public class EthereumTransportUpdateDaoTest
    {
        private EthereumTransportUpdateDao _dao;

        private Mock<IContract> _contractMock;
        private Mock<IFunction> _functionMock;

        [SetUp]
        public void Init()
        {
            _dao = new EthereumTransportUpdateDao();

            _contractMock = new Mock<IContract>();
            _dao.Contract = _contractMock.Object;

            _functionMock = new Mock<IFunction>();
            _contractMock.Setup(m => m.GetFunction(It.IsAny<string>())).Returns(_functionMock.Object);
        }

        [Test]
        public void ShouldCallCorrectFunctions()
        {
            var transport = new Transport
            {
                Transporter = "Transporter1",
                PickupDate = new DateTime(2018, 06, 08, 11, 00, 00),
                DeliverDate = new DateTime(2018, 06, 08, 12, 00, 00)
            };

            var update = new TransportUpdate
            {
                TransactionId = "62FA647C-AD54-4BCC-A860-E5A2664B019D",
                Transport = transport
            };

            _dao.ExecuteTransportUpdate(update);

            _contractMock.Verify(m => m.GetFunction("updateTransport"));
            _functionMock.Verify(m => m.SendTransactionAsync(
                It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "62FA647C-AD54-4BCC-A860-E5A2664B019D", 
                "Transporter1",
                1528455600,
                1528459200
            ));
        }

        [Test]
        public void ShouldCallFunctionWithZeroAsTimestampWhenTimestampIsNotGiven()
        {
            var transport = new Transport
            {
                Transporter = "Transporter1",
                DeliverDate = null,
                PickupDate = null
            };

            var update = new TransportUpdate
            {
                TransactionId = "62FA647C-AD54-4BCC-A860-E5A2664B019D",
                Transport = transport
            };

            _dao.ExecuteTransportUpdate(update);

            _contractMock.Verify(m => m.GetFunction("updateTransport"));
            _functionMock.Verify(m => m.SendTransactionAsync(
                It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "62FA647C-AD54-4BCC-A860-E5A2664B019D", 
                "Transporter1",
                0,
                0
            ));
        }
    }
}
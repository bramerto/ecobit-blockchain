using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;
using Ecobit_Blockchain_Frontend.Controllers;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils.Auth;
using Ecobit_Blockchain_Frontend.Models.View;
using Moq;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Controllers
{
    [TestFixture]
    public class BatchControllerTest
    {
        private BatchController _controller;
        private Mock<ITransactionDao> _mock;
        
        [SetUp]
        public void Init()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    BatchId = 1,
                    FromOwner = "Owner2",
                    ToOwner = "Owner3",
                    OrderTime = new DateTime(2018, 5, 24, 15, 00, 00)
                },
                new Transaction
                {
                    BatchId = 1,
                    FromOwner = "Owner1",
                    ToOwner = "Owner2",
                    OrderTime = new DateTime(2018, 5, 24, 14, 00, 00)
                }
            };

            _mock = new Mock<ITransactionDao>();
            _mock.Setup(dao => dao.GetTransactionsByBatchId(1)).Returns(transactions);
            _mock.Setup(dao => dao.GetTransactionsByUser("Test")).Returns(transactions);
            
            var authMock = new Mock<IAuthenticationService>();
            authMock.Setup(mock => mock.GetUserData(It.IsAny<HttpCookie>()))
                .Returns(new User {Companyname = "Test", Password = "test"});
            
            _controller = new BatchController(_mock.Object, authMock.Object);
        }

        [Test]
        public void ControllerShouldNotThrowException()
        {
            _controller.History(1);
        }

        [Test]
        public void ControllerShouldNotThrowExceptionUserHistory()
        {
            _controller.UserHistory(null);
        }

        [Test]
        public void ControllerShouldCallGetTransactionsWithOne()
        {
            _controller.History(1);
            
            _mock.Verify(dao => dao.GetTransactionsByBatchId(1));
        }

        [Test]
        public void ControllerShouldCallGetTransactionsWithOneUserHistory()
        {
            _controller.UserHistory(null);
            
            _mock.Verify(dao => dao.GetTransactionsByUser("Test"));
        }
        
        [Test]
        public void ControllerShouldReturnAResult()
        {
            var result = _controller.History(1);
            
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }
        
        [Test]
        public void ControllerShouldReturnAResultUserHistory()
        {
            var result = _controller.UserHistory(null);
            
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ControllerShouldReturnHistoryAsModel()
        {
            var result = (ViewResult) _controller.History(1);
            
            Assert.IsInstanceOf<HistoryView>(result.ViewData.Model);
        }
        
        [Test]
        public void ControllerShouldReturnUserHistoryAsModel()
        {
            var result = (ViewResult) _controller.UserHistory(null);
            
            Assert.IsInstanceOf<UserHistory>(result.ViewData.Model);
        }

        [Test]
        public void ControllerShouldReturnTransactionsAndUserOne()
        {
            var result = (ViewResult) _controller.UserHistory(null);
            var model = (UserHistory) result.ViewData.Model;
            
            Assert.AreEqual("Test", model.User);
            Assert.IsInstanceOf<List<Transaction>>(model.Transactions);
        }
        
        [Test]
        public void ControllerShouldServeIndexPage()
        {
            var result = (ViewResult) _controller.Index();
            Assert.AreEqual("", result.ViewName);
        }

        [Test]
        public void ControllerShouldRedirectToHistoryPage()
        {
            var result = (RedirectToRouteResult) _controller.Index(new Batch {BatchId = 1});
            Assert.AreEqual("History", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["id"]);
        }

        [Test]
        public void TestCreateBatchPage()
        {
            var result = (ViewResult) _controller.Create();
            Assert.AreEqual(result.ViewName, string.Empty);
        }
        
        [Test]
        public void TestCreateBatch()
        {
            var result = (ViewResult) _controller.Create();
            Assert.AreEqual(result.ViewName, string.Empty);
        }
        
        [Test]
        public void TestCreateBatchFail()
        {
            var transaction = new Transaction();
            
            _controller.Create(transaction);
            
            _mock.Verify(dao => dao.SaveTransaction(transaction));
        }
    }
}
using System;
using System.Collections.Generic;
using Ecobit_Blockchain_Frontend.Factories;
using Ecobit_Blockchain_Frontend.Models;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Factories
{
    [TestFixture]
    public class SupplyChainFactoryTest
    {
        [Test]
        public void ShouldOrderTheTransactionsByDate()
        {
            var transactions = new List<Transaction>
            {
                new Transaction {OrderTime = new DateTime(2002, 1, 1), FromOwner = "test2", ToOwner = "test3"},
                new Transaction {OrderTime = new DateTime(2001, 1, 1), FromOwner = "test2", ToOwner = "test3"},
                new Transaction {OrderTime = new DateTime(2000, 1, 1), FromOwner = "test1", ToOwner = "test2"}
            };


            var chain = SupplyChainFactory.Make(transactions);
            
            Assert.AreEqual(2001, chain.Children[0].Transaction.OrderTime.Year);            
        }
        
        [Test]
        public void ShouldHaveTheCorrectChildren()
        {
            var transactions = new List<Transaction>
            {
                new Transaction {OrderTime = new DateTime(2000, 1, 1), FromOwner = "test1", ToOwner = "test2"},
                new Transaction {OrderTime = new DateTime(2001, 1, 1), FromOwner = "test2", ToOwner = "test3"},
                new Transaction {OrderTime = new DateTime(2002, 1, 1), FromOwner = "test3", ToOwner = "test4"}
            };


            var chain = SupplyChainFactory.Make(transactions);

            Assert.AreEqual("test1", chain.Transaction.FromOwner);
            Assert.AreEqual("test2", chain.Children[0].Transaction.FromOwner);
            Assert.AreEqual("test3", chain.Children[0].Children[0].Transaction.FromOwner);
            
        }
    }
}
using System;
using System.Linq;
using Application.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using NUnit.Framework;
using Validator = Microsoft.Practices.EnterpriseLibrary.Validation.Validator;

namespace Application.Tests.Models
{
    public class TransactionTest
    {
        
        public ValidationResults Validate(object model)
        {
            Validator validator = ValidationFactory.CreateValidator<Transaction>();
            var results = new ValidationResults();
            validator.Validate(model, results);
            
            return results;
        }
        
        [Test]
        public void ValidateCorrectTransactionModel()
        {
            var transaction = new Transaction
            {
                BatchId = 1,
                TransactionId = "testid",
                Quantity = 2,
                OrderTime = DateTime.Now,
                ItemPrice = 3,
                From = "Groothandel",
                To = "Winkel",
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transaction);
            
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public void ValidateNullValuesInTransactionModel()
        {
            var transaction = new Transaction
            {
                BatchId = 1,
                Quantity = 1,
                ItemPrice = 1,
                OrderTime = DateTime.Now
            };

            var results = Validate(transaction).ToList();
            
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("TransactionId cannot be null.", results[0].Message);
            Assert.AreEqual("From cannot be null.", results[1].Message);
            Assert.AreEqual("To cannot be null.", results[2].Message);
        }
        
        [Test]
        public void ValidateInCorrectBatchIdTransactionIdItemPriceInTransactionModel()
        {
            var transaction = new Transaction
            {
                BatchId = -1,
                TransactionId = "testid",
                Quantity = -2,
                OrderTime = DateTime.Now,
                ItemPrice = -3,
                From = "Groothandel",
                To = "Winkel",
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transaction).ToList();
            
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("BatchId needs to be greater than 0.", results[0].Message);
            Assert.AreEqual("Quantity needs to be greater than 0.", results[1].Message);
            Assert.AreEqual("ItemPrice cannot be negative.", results[2].Message);
        }
        
        [Test]
        public void ValidateInCorrectOrderTimeInTransactionModel()
        {
            var transaction = new Transaction
            {
                BatchId = 1,
                TransactionId = "testid",
                Quantity = 2,
                ItemPrice = 3,
                From = "Groothandel",
                To = "Winkel",
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transaction).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("OrderTime needs to have a date.", results[0].Message);
        }
        
        [Test]
        public void ValidateInCorrectOrderTimeInTransactionModelWithTransportModel()
        {
            var transaction = new Transaction
            {
                BatchId = 1,
                TransactionId = "testid",
                Quantity = 2,
                ItemPrice = 3, 
                OrderTime = DateTime.Now.AddHours(3),
                From = "Groothandel",
                To = "Winkel",
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transaction).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("OrderTime date cannot be after the pickup and deliverdate.", results[0].Message);
        }
    }
}
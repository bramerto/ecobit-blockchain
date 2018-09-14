using System;
using System.Linq;
using Application.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using NUnit.Framework;

namespace Application.Tests.Models
{
    public class TransportUpdateTest
    {
        
        public ValidationResults Validate(object model)
        {
            Validator validator = ValidationFactory.CreateValidator<TransportUpdate>();
            var results = new ValidationResults();
            validator.Validate(model, results);
            
            return results;
        }

        [Test]
        public void ValidateCorrectUpdateTransportUpdateModel()
        {
            var transportUpdate = new TransportUpdate
            {
                TransactionId = "1",
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transportUpdate);
            
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public void ValidateInCorrectTransactionIdInTransportUpdateModel()
        {
            var transportUpdate = new TransportUpdate
            {
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transportUpdate).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("TransactionId cannot be null.", results[0].Message);
        }
        
        [Test]
        public void ValidateInCorrectTransportInTransportUpdateModel()
        {
            var transportUpdate = new TransportUpdate
            {
                Transport = new Transport
                {
                    Transporter = "DHL",
                    PickupDate = DateTime.Now,
                    DeliverDate = DateTime.Now.AddHours(1)
                }
            };

            var results = Validate(transportUpdate).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("TransactionId cannot be null.", results[0].Message);
        }
        
        [Test]
        public void ValidateInCorrectPickUpDateTransportModel()
        {
            var transportUpdate = new TransportUpdate
            {
                TransactionId = "1",
                Transport = new Transport
                {
                     Transporter = "DHL",
                     PickupDate = DateTime.Now.AddHours(1),
                     DeliverDate = DateTime.Now
                }
                    
            };

            var results = Validate(transportUpdate).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Pickup date needs to be before deliver date.", results[0].Message);
        }
    }
}
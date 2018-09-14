using System;
using System.Linq;
using Application.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using NUnit.Framework;

namespace Application.Tests.Models
{
    public class TransportTest
    {
        
        public ValidationResults Validate(object model)
        {
            Validator validator = ValidationFactory.CreateValidator<Transport>();
            var results = new ValidationResults();
            validator.Validate(model, results);
            
            return results;
        }

        [Test]
        public void ValidateCorrectTransportModel()
        {
            var transport = new Transport
            {
                Transporter = "DHL",
                PickupDate = DateTime.Now,
                DeliverDate = DateTime.Now.AddHours(1)
            };

            var results = Validate(transport);
            
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public void ValidateInCorrectPickUpDateTransportModel()
        {
            var transport = new Transport
            {
                Transporter = "DHL",
                PickupDate = DateTime.Now.AddHours(1),
                DeliverDate = DateTime.Now
            };

            var results = Validate(transport).ToList();
            
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Pickup date needs to be before deliver date.", results[0].Message);
        }
    }
}
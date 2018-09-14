using Application.Models;
using Application.Validation;
using NUnit.Framework;

namespace Application.Tests.Validation
{
    public class TransportUpdateValidatorTest
    {
        [Test]
        public void ThatTransportUpdateGetCorrectAmountOfResults()
        {
            TransportUpdateValidator validator = new TransportUpdateValidator();
            validator.Validate(new TransportUpdate());
            
            Assert.AreEqual(1, validator.GetResults().Count);   
        }
    }
}
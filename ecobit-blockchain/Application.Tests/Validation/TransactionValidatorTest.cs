using Application.Models;
using Application.Validation;
using NUnit.Framework;

namespace Application.Tests.Validation
{
    public class TransactionValidatorTest
    {
        [Test]
        public void ThatTransactionGetCorrectAmountOfResults()
        {
            var validator = new TransactionValidator();
            validator.Validate(new Transaction());
            
            Assert.AreEqual(7, validator.GetResults().Count);
        }
    }
}
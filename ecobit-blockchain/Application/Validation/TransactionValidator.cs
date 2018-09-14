using Application.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Application.Validation
{
    public  class TransactionValidator : IValidator<Transaction>
    {
        private readonly Validator _validator;
        private ValidationResults Results { get; }

        public TransactionValidator()
        {
            _validator = ValidationFactory.CreateValidator<Transaction>();
            Results = new ValidationResults();
        }

        public void Validate(Transaction entity)
        {
            _validator.Validate(entity, Results);
        }

        public ValidationResults GetResults()
        {
            return Results;
        }
    }
}
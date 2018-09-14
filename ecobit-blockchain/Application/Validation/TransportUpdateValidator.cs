using Application.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Application.Validation
{
    public class TransportUpdateValidator : IValidator<TransportUpdate>
    {
        private readonly Validator _validator;
        private ValidationResults Results { get; }

        public TransportUpdateValidator()
        {
            _validator = ValidationFactory.CreateValidator<TransportUpdate>();
            Results = new ValidationResults();
        }

        public void Validate (TransportUpdate entity)
        {
            _validator.Validate(entity, Results);
        }
        
        public ValidationResults GetResults()
        {
            return Results;
        }
    }
}
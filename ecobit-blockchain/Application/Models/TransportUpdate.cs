using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Application.Models
{
    public class TransportUpdate
    {
        [NotNullValidator(MessageTemplate = "TransactionId cannot be null.")]
        public string TransactionId { get; set; }
        
        [ObjectValidator]
        public Transport Transport { get; set; }
    }
}

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Application.Models
{
    /// <summary>
    /// Data object to store transports.
    /// </summary>
    [HasSelfValidation]
    public class Transport
    {
        public string Transporter { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? DeliverDate { get; set; }
        
        [SelfValidation]
        public void ValidateDate(ValidationResults results)
        {
            if (DeliverDate != null && PickupDate != null && PickupDate > DeliverDate)
            {
                results.AddResult(new ValidationResult("Pickup date needs to be before deliver date.", this, "", "",
                    null));
            }
        }
    }
    
    
}
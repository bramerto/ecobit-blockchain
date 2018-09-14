using System;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace Application.Models
{
    /// <summary>
    /// Data object to store transactions
    /// </summary>
    [HasSelfValidation]
    public class Transaction
    {
        
        public int BatchId { get; set; }
        
        [NotNullValidator(MessageTemplate = "TransactionId cannot be null.")]
        public string TransactionId { get; set; }
        
        public int Quantity { get; set; }
        
        public DateTime OrderTime { get; set; }
        
        public double ItemPrice { get; set; }
        
        [NotNullValidator(MessageTemplate = "From cannot be null.")]
        public string From { get; set; }
        
        [NotNullValidator(MessageTemplate = "To cannot be null.")]
        public string To { get; set; }
        
        [ObjectValidator]
        public Transport Transport { get; set; }
        
        
        [SelfValidation]
        public void ValidateBatchIdAndQuantityAndItemPrice(ValidationResults results)
        {
            if (BatchId <= 0)
            {
                results.AddResult(new ValidationResult("BatchId needs to be greater than 0.",this,"","",null));
            }

            if (Quantity <= 0)
            {
                results.AddResult(new ValidationResult("Quantity needs to be greater than 0.",this,"","",null));
            }
            
            if (ItemPrice <= 0)
            {
                results.AddResult(new ValidationResult("ItemPrice cannot be negative.",this,"","",null));
            }
        }

        [SelfValidation]
        public void ValidateOrderTime(ValidationResults results)
        {
            if (OrderTime == DateTime.MinValue)
            {
                results.AddResult(new ValidationResult("OrderTime needs to have a date.",this,"","",null));
            }

            if (Transport != null && (OrderTime > Transport.DeliverDate || OrderTime > Transport.PickupDate))
            {
                results.AddResult(new ValidationResult("OrderTime date cannot be after the pickup and deliverdate.",this,"","",null));
            }
        }
    }
}
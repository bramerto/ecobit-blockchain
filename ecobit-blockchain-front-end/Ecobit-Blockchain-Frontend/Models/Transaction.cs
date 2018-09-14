using System;
using System.ComponentModel.DataAnnotations;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class Transaction
    {
        [Required (ErrorMessage = "Partijnummer niet neergezet")]
        [Display(Name = "Partijnummer")]
        public int BatchId { get; set; }
        
        public string TransactionId { get; set; }
        
        [Required (ErrorMessage = "Quantiteit niet neergezet")]
        [Display(Name = "Quantiteit")]
        public int Quantity { get; set; }
        
        [Required (ErrorMessage = "Prijs per item niet neergezet")]
        [Display(Name = "Prijs per item")]
        public int ItemPrice { get; set; }
        
        [Required (ErrorMessage = "Datum van order niet neergezet")]
        [Display(Name = "Datum van order")]
        public DateTime OrderTime { get; set; }
        
        [Required (ErrorMessage = "Verzender van niet neergezet")]
        [Display(Name = "Verzender")]
        public string FromOwner { get; set; }
        
        [Required (ErrorMessage = "Ontvanger niet neergezet")]
        [Display(Name = "Ontvanger")]
        public string ToOwner { get; set; }
        
        public Transport Transport { get; set; }

    }
}
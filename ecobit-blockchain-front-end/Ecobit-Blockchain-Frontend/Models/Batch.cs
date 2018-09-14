using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class Batch
    {
        [Required(ErrorMessage = "Partijnummer is verplicht")]
        [Display(Name = "Partijnummer")]
        [XmlElement("id")]
        public int BatchId { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("date_production")]
        public string ProductionDate { get; set; }

        [XmlElement("owner")]
        public string Owner { get; set; }
    }
}
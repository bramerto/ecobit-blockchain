using System;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class Transport
    {
        public string Transporter { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliverDate { get; set; }
    }
}
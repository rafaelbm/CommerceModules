using System;

namespace Commerce.Common.Entities
{
    public class PurchasedItem
    {
        public int Sku { get; set; }
        public double PurchasePrice { get; set; }
        public DateTime PurchasedOn { get; set; }
    }
}

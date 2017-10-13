using Commerce.Common.Modules;

namespace Commerce.Modules
{
    public class ItemPromotionModule : ICommerceModule
    {
        public void Initialize(CommerceEvents events)
        {
            events.OrderItemProcessed += OrderItemProcessed;
        }

        private void OrderItemProcessed(OrderItemProcessedEventArgs e)
        {
            if (e.OrderLineItemData.Sku == 102)
            {
                e.OrderLineItemData.PurchasePrice -= 30.00;
            }
        }
    }
}

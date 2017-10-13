using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commerce.Common.Modules;

namespace Commerce.Modules
{
    public class SkuMinerModule : ICommerceModule
    {
        public void Initialize(CommerceEvents events)
        {
            events.OrderItemProcessed += OrderItemProcessed;
        }

        private void OrderItemProcessed(OrderItemProcessedEventArgs e)
        {
            if (e.OrderLineItemData.Sku == 101)
            {
                Console.WriteLine("Sku 101 was purchassed on {0}", DateTime.Now);
            }
        }
    }
}

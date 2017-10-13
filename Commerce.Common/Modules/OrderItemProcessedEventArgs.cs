using System.ComponentModel;
using Commerce.Common.Data_Models;
using Commerce.Common.Entities;

namespace Commerce.Common.Modules
{
    public class OrderItemProcessedEventArgs : CancelEventArgs
    {
        public Customer Customer { get; set; }
        public OrderLineItemData OrderLineItemData { get; set; }
        public string MessageText { get; set; }

        public OrderItemProcessedEventArgs(Customer customer, OrderLineItemData orderLineItemData)
        {
            Customer = customer;
            OrderLineItemData = orderLineItemData;
        }
    }
}
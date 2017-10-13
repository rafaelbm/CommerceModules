using System;
using System.Linq;
using System.Transactions;
using Commerce.Common.Contracts;
using Commerce.Common.Data_Models;
using Commerce.Common.Entities;
using Commerce.Common.Modules;
using Commerce.Engine.Contracts;

namespace Commerce.Engine
{
    public class CommerceManager : ICommerceManager
    {
        public CommerceManager(IStoreRepository storeRepository, IConfigurationFactory configurationFactory)
        {
            _storeRepository = storeRepository;
            
            // load providers
            _paymentProcessor = configurationFactory.GetPaymentProcessor();
            _mailer = configurationFactory.GetMailer();
            _commerceEvents = configurationFactory.GetEvents();
        }

        readonly IStoreRepository _storeRepository;
        readonly IPaymentProcessor _paymentProcessor;
        readonly IMailer _mailer;
        readonly CommerceEvents _commerceEvents;
        
        #region ICommerceManager Members

        public void ProcessOrder(OrderData orderData)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Customer customer = _storeRepository.GetCustomerByEmail(orderData.CustomerEmail);
                    if (customer == null)
                        throw new ApplicationException($"No customer on file with email {orderData.CustomerEmail}.");

                    // Decrease product inventories
                    foreach (OrderLineItemData lineItem in orderData.LineItems)
                    {
                        if (_commerceEvents.OrderItemProcessed != null)
                        {
                            OrderItemProcessedEventArgs args =
                                new OrderItemProcessedEventArgs(customer, lineItem);
                            _commerceEvents.OrderItemProcessed(args);

                            if (args.Cancel)
                            {
                                throw new ApplicationException(args.MessageText);
                            }
                        }

                        Product product = _storeRepository.Products.FirstOrDefault(item => item.Sku == lineItem.Sku);
                        if (product == null)
                            throw new ApplicationException($"Sku {lineItem.Sku} not found in store inventory.");

                        Inventory inventoryOnHand = _storeRepository.ProductInventory.FirstOrDefault(item => item.Sku == lineItem.Sku);
                        if (inventoryOnHand == null)
                            throw new ApplicationException(
                                $"Error attempting to determine on-hand inventory quantity for product {lineItem.Sku}.");

                        if (inventoryOnHand.QuantityInStock < lineItem.Quantity)
                            throw new ApplicationException(
                                $"Not enough quantity on-hand to satisfy product {lineItem.Sku} purchase of {lineItem.Quantity} units.");

                        inventoryOnHand.QuantityInStock -= lineItem.Quantity;
                        Console.WriteLine("Inventory for product {0} reduced by {1} units.", lineItem.Sku, lineItem.Quantity);
                    }

                    // Update customer records with purchase
                    foreach (OrderLineItemData lineItem in orderData.LineItems)
                    {
                        for (int i = 0; i < lineItem.Quantity; i++)
                            customer.Purchases.Add(new PurchasedItem() { Sku = lineItem.Sku, PurchasePrice = lineItem.PurchasePrice, PurchasedOn = DateTime.Now });
                        Console.WriteLine("Added {0} unit(s) or product {1} to customer's purchase history.", lineItem.Quantity, lineItem.Sku);
                    }

                    // Process customer credit card
                    double amount = orderData.LineItems.Sum(lineItem => (lineItem.PurchasePrice*lineItem.Quantity));

                    bool paymentSuccess = _paymentProcessor.ProcessCreditCard(customer.Name, orderData.CreditCard, orderData.ExpirationDate, amount);
                    if (!paymentSuccess)
                        throw new ApplicationException($"Credit card {orderData.CreditCard} could not be processed.");

                    // Send invoice email
                    _mailer.SendInvoiceEmail(orderData);

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                _mailer.SendRejectionEmail(orderData);
                throw;
            }
        }

        #endregion
    }
}

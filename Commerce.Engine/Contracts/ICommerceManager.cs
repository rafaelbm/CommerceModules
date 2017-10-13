using Commerce.Common.Data_Models;

namespace Commerce.Engine.Contracts
{
    public interface ICommerceManager
    {
        void ProcessOrder(OrderData orderData);
    }
}

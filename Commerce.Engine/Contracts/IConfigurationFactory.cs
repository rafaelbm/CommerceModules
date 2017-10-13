using Commerce.Common.Contracts;
using Commerce.Common.Modules;

namespace Commerce.Engine.Contracts
{
    public interface IConfigurationFactory
    {
        IPaymentProcessor GetPaymentProcessor();
        IMailer GetMailer();
        CommerceEvents GetEvents();
    }
}

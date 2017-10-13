using System;
using System.Configuration;
using Commerce.Engine.Configuration;
using Commerce.Engine.Contracts;
using Commerce.Common.Contracts;
using Commerce.Common.Modules;

namespace Commerce.Engine
{
    // this class can be singleton'd by the DI container
    public class ConfigurationFactory : IConfigurationFactory
    {
        public ConfigurationFactory()
        {
            CommerceEngineConfigurationSection config = ConfigurationManager.GetSection("commerceEngine") as CommerceEngineConfigurationSection;
            if (config != null)
            {
                IPaymentProcessor paymentProcessor = Activator.CreateInstance(Type.GetType(config.PaymentProcessor.Type)) as IPaymentProcessor;
                IMailer mailer = Activator.CreateInstance(Type.GetType(config.Mailer.Type)) as IMailer;
                mailer.FromAddress = config.Mailer.FromAddress;
                mailer.SmtpServer = config.Mailer.SmtpServer;

                _paymentProcessor = paymentProcessor;
                _mailer = mailer;

                _events = new CommerceEvents();

                foreach (ModuleElement moduleElement in config.Modules)
                {
                    ICommerceModule module =
                        Activator.CreateInstance(Type.GetType(moduleElement.Type)) as ICommerceModule;
                    module.Initialize(_events);
                }
            }
        }

        readonly IPaymentProcessor _paymentProcessor;
        readonly IMailer _mailer;
        readonly CommerceEvents _events;

        IPaymentProcessor IConfigurationFactory.GetPaymentProcessor()
        {
            return _paymentProcessor;
        }

        IMailer IConfigurationFactory.GetMailer()
        {
            return _mailer;
        }

        CommerceEvents IConfigurationFactory.GetEvents()
        {
            return _events;
        }
    }
}

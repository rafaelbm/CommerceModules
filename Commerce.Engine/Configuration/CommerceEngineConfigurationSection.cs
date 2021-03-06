﻿using System.Configuration;

namespace Commerce.Engine.Configuration
{
    public class CommerceEngineConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("paymentProcessor", IsRequired = true)]
        public PaymentProcessorElement PaymentProcessor
        {
            get { return (PaymentProcessorElement)base["paymentProcessor"]; }
            set { base["paymentProcessor"] = value; }
        }

        [ConfigurationProperty("mailer", IsRequired = true)]
        public MailerElement Mailer
        {
            get { return (MailerElement)base["mailer"]; }
            set { base["mailer"] = value; }
        }

        [ConfigurationProperty("modules", IsRequired = true)]
        public ModuleElementCollection Modules
        {
            get { return (ModuleElementCollection)base["modules"]; }
            set { base["modules"] = value; }
        }
    }
}

using Commerce.Common.Data_Models;

namespace Commerce.Common.Contracts
{
    public interface IMailer
    {
        void SendInvoiceEmail(OrderData orderData);
        void SendRejectionEmail(OrderData orderData);

        string FromAddress { get; set; }
        string SmtpServer { get; set; }
    }
}

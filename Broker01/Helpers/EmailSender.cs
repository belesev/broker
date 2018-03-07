using System;
using System.Net;
using System.Net.Mail;

namespace BrokerAlgo.Helpers
{
    public class EmailSender : IDisposable
    {
        private readonly SmtpClient smtpClient;
        private readonly MailAddress addressFrom;
        private readonly MailAddress addressTo;

        public EmailSender(string stmpHostname, int smtpPort, string smtpFrom, string smtpPassword)
        {
            addressFrom = new MailAddress(smtpFrom);
            addressTo = new MailAddress(smtpFrom);

            smtpClient = new SmtpClient(stmpHostname, smtpPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(addressFrom.Address, smtpPassword)
            };
        }

        public void Send(string subject, string body)
        {
            using (var msg = new MailMessage(addressFrom, addressTo)
            {
                Subject = subject,
                Body = body
            })
            {
                smtpClient.Send(msg);
            }
        }

        public void Dispose()
        {
            smtpClient?.Dispose();
        }
    }
}
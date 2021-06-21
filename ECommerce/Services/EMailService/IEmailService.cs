using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.EMailService
{
    public interface IEmailService
    {
        void SendPlainTextGmail(string resipientEmail, string recipientName);
        void SendHtmlGmail(string resipientEmail, string recipientName, string subject, int version, object data);
        void SendHtmlWithAttachemntGmail(string resipientEmail, string recipientName);
    }
}

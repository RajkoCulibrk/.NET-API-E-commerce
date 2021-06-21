using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ECommerce.Services.EMailService
{
    public class EmailService : IEmailService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void SendHtmlGmail(string resipientEmail, string recipientName,string subject,int version,object data)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();
                var email = mailer
                                 .To(resipientEmail, recipientName);
                if (version == 1)
                {
                    mailer
                       .Subject(subject)
                       .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/EmailTemplates/SuccessRegistration.cshtml",
                                           new
                                           {
                                               Name = recipientName
                                           });
                }
                if (version == 2)
                {
                     mailer
                        .Subject(subject)
                        .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/EmailTemplates/SuccessfullOrder.cshtml",
                                           data
                                           );
                }
                if (version == 3)
                {
                    mailer
                        .Subject(subject)
                        .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/EmailTemplates/ConfirmSend.cshtml",
                                           data
                                           );
                }
                if (version == 4)
                {
                    mailer
                        .Subject(subject)
                        .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/EmailTemplates/Invoice.cshtml",
                                           data
                                           );
                }

                await email.SendAsync();
            }
        }

        public async void SendHtmlWithAttachemntGmail(string resipientEmail, string recipientName)
        {
            throw new NotImplementedException();
        }

        public async void SendPlainTextGmail(string resipientEmail, string recipientName)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();
                var email = mailer
                                 .To(resipientEmail, recipientName)
                                 .Subject("Well hello there")
                                 .Body("this is just a test");

                await email.SendAsync();

            }
        }
    }
}

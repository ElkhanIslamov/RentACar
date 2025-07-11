﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Mailing.MailKitImplementations
{
    public class MailKitMailService: IMailService
    {
        private readonly MailSettings? _mailSettings;

        public MailKitMailService(IConfiguration configuration)
        {
            _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        }

        public void SendMail(Mail mail)
        {
            if (mail == null) throw new ArgumentNullException("Email not defined.");

            if (_mailSettings == null) throw new ArgumentNullException("Mail settting not defined.");

            MimeMessage email = new();

            email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));

            email.To.Add(new MailboxAddress(mail.ToFullName, mail.ToEmail));

            email.Subject = mail.Subject;

            BodyBuilder bodyBuilder = new()
            {
                TextBody = mail.TextBody,
                HtmlBody = mail.HtmlBody
            };           

            email.Body = bodyBuilder.ToMessageBody();

            using SmtpClient smtp = new();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
   
}

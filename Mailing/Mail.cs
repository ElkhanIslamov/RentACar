﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mailing
{
    public class Mail
    {
        public string? Subject { get; set; }
        public string? TextBody { get; set; }
        public string? HtmlBody { get; set; }
        public AttachmentCollection? Attachments { get; set; }
        public string? ToFullName { get; set; }
        public required string ToEmail { get; set; }

        public Mail()
        {
        }

        public Mail(string subject, string textBody, string htmlBody, AttachmentCollection? attachments, string toFullName, string toEmail)
        {
            Subject = subject;
            TextBody = textBody;
            HtmlBody = htmlBody;
            Attachments = attachments;
            ToFullName = toFullName;
            ToEmail = toEmail;
        }
    }
}

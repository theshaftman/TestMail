using EAGetMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailProject.Models.DataViews
{
    internal class DataModel
    {
        public MailAddress From { get; set; }
        public MailAddress[] To { get; set; }
        public MailAddress[] Cc { get; set; }
        public MailAddress[] Bcc { get; set; }
        public DataAttachment[] Attachments { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime SentDate { get; set; }
    }
}
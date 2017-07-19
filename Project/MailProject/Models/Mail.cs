using EAGetMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MailProject.Models
{
    internal class Mail
    {
        internal MailInfo[] GetAllMails()
        {
            // Gmail IMAP4 server is "imap.gmail.com"
            MailServer oServer = new MailServer("imap.gmail.com",
                        Constant.USERNAME, Constant.PASSWORD, ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            // Set SSL connection,
            oServer.SSLConnection = true;

            // Set 993 IMAP4 port
            oServer.Port = 993;

            try
            {
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                
                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
                return infos;
            }
            catch (Exception ep)
            {
                return null;
            }
        }

        internal IEnumerable<EAGetMail.Mail> GetMails()
        {
            // Gmail IMAP4 server is "imap.gmail.com"
            MailServer oServer = new MailServer("imap.gmail.com",
                        Constant.USERNAME, Constant.PASSWORD, ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            // Set SSL connection,
            oServer.SSLConnection = true;

            // Set 993 IMAP4 port
            oServer.Port = 993;

            try
            {
                IList<EAGetMail.Mail> list = new List<EAGetMail.Mail>();
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();

                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];
                    // Download email from GMail IMAP4 server
                    EAGetMail.Mail oMail = oClient.GetMail(info);
                    list.Add(oMail);
                }

                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
                list = list.OrderBy(m => m.Subject).ThenBy(m => m.ReplyTo.Address).ToList();
                return list;
            }
            catch (Exception ep)
            {
                return null;
            }
        }

        internal bool SendMail(string to, string subject, string body)
        {
            var message = new MailMessage(Constant.USERNAME, to);
            message.Subject = subject;
            message.Body = body;
            SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587);
            mailer.Credentials = new NetworkCredential(Constant.USERNAME, Constant.PASSWORD);
            mailer.EnableSsl = true;
            mailer.Send(message);
            return true;
        }
    }
}
using EAGetMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using MailProject.Models.Interfaces;
using MailProject.Models.DataViews;

namespace MailProject.Models.Data
{
    internal class MailModel : IMailModel
    {
        MailInfo[] IMailModel.GetAllMails()
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

        IEnumerable<DataModel> IMailModel.GetMails()
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
                IList<DataModel> list = new List<DataModel>();
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                Imap4Folder[] folders = oClient.Imap4Folders;

                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];
                    // Download email from GMail IMAP4 server
                    EAGetMail.Mail oMail = oClient.GetMail(info);
                    var attachments = new DataAttachment[oMail.Attachments.Length];
                    try
                    {
                        for (int j = 0; j < attachments.Length; j++)
                        {
                            attachments[j].ContentID = oMail.Attachments[j].ContentID;
                            attachments[j].Name = oMail.Attachments[j].Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                    }
                    var oMailData = new DataModel()
                    {
                        From = oMail.From,
                        To = oMail.To,
                        Cc = oMail.Cc,
                        Bcc = oMail.Bcc,
                        Subject = oMail.Subject,
                        TextBody = oMail.TextBody,
                        HtmlBody = oMail.HtmlBody,
                        SentDate = oMail.SentDate,
                        ReceivedDate = oMail.ReceivedDate,
                        Attachments = attachments
                    };
                    list.Add(oMailData);
                }

                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
                list = OrderList(list);
                return list;
            }
            catch (Exception ep)
            {
                return null;
            }
        }
        
        Mail IMailModel.GetMail(int id)
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
                MailInfo info = infos.Where(m => m.Index.Equals(id)).FirstOrDefault();

                // Download email from GMail IMAP4 server
                EAGetMail.Mail oMail = oClient.GetMail(info);                

                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
                return oMail;
            }
            catch (Exception ep)
            {
                return null;
            }
        }

        bool IMailModel.SendMail(string to, string subject, string body)
        {
            var message = new MailMessage(Constant.USERNAME, to);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587);
            mailer.Credentials = new NetworkCredential(Constant.USERNAME, Constant.PASSWORD);
            mailer.EnableSsl = true;
            mailer.Send(message);
            return true;
        }

        private IList<DataModel> OrderList(IList<DataModel> list)
        {
            var listSubjects = list
                .OrderByDescending(m => m.ReceivedDate)
                .Select(m => m.Subject.ToUpper().Replace("RE: ", "").Replace("FWD: ", ""))
                .Distinct()
                .ToList();
            var orderedList = new List<DataModel>();
            foreach (var item in listSubjects)
            {
                orderedList.AddRange(list
                    .OrderBy(m => m.ReceivedDate)
                    .Where(m => m.Subject.ToUpper().Replace("RE: ", "").Replace("FWD: ", "")
                    .Equals(item)));
            }
            return orderedList;
        }
    }
}
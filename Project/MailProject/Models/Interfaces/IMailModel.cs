using EAGetMail;
using MailProject.Models.DataViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailProject.Models.Interfaces
{
    internal interface IMailModel
    {
        MailInfo[] GetAllMails();
        IEnumerable<DataModel> GetMails();
        Mail GetMail(int id);
        bool SendMail(string to, string subject, string body);
    }
}
using MailProject.Models.Data;
using MailProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailProject.Controllers
{
    public class HomeController : Controller
    {
        private IMailModel mail;

        public HomeController()
        {
            this.mail = new MailModel();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetEmails()
        {
            return Json(new
            {
                data = this.mail.GetMails()
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetEmail(int id)
        {
            return Json(new
            {
                data = this.mail.GetMail(id)
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost, ValidateInput(false)]
        public ActionResult SendMail(string to, string subject, string body)
        {
            return Json(new
            {
                status = this.mail.SendMail(to, subject, body)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
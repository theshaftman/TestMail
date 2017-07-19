using MailProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailProject.Controllers
{
    public class HomeController : Controller
    {
        private Mail mail;

        public HomeController()
        {
            this.mail = new Mail();
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
        [HttpPost]
        public ActionResult SendMail(string to, string subject, string body)
        {
            return Json(new
            {
                status = this.mail.SendMail(to, subject, body)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
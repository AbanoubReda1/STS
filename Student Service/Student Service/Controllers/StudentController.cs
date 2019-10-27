using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Student_Service.Models;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace Student_Service.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        [HttpGet]

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "E_MailVerified,ActivationCode")]Student student)
        {
            bool Status = false;
            string Message = " ";

            if (ModelState.IsValid)
            {
                var isExsist = IsExsist(student.E_Mail);
                if (isExsist)
                {
                    ModelState.AddModelError("EmailExist", "Email is already Exist");

                    return View(student);
                }

                #region Generate Activation Code
                student.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                student.Password = Crypto.Hash(student.Password);
                student.ConfiremPassword = Crypto.Hash(student.ConfiremPassword);
                #endregion
                student.E_MailVerified = false;

                #region save data to data base
                using (MydataEntities dc = new MydataEntities())
                {
                    dc.Students.Add(student);
                    dc.SaveChanges();
                    //Send Email to User
                    sendemailverifiction(student.E_Mail, student.ActivationCode.ToString());
                    Message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + student.E_Mail;
                    Status = true;

                }

                #endregion
            }
            else
            {
                Message = "Invalid Request";

            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;

            return View(student);

        }

        //verfyaccount
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MydataEntities dc = new MydataEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //to avoid

                var v = dc.Students.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.E_MailVerified = true;
                    dc.SaveChanges();
                    Status = true;

                }
                else
                {
                    ViewBag.Message = "Invalid Request";

                }

            }
            ViewBag.Status = Status;
            return View();

        }
        //login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //logout
      

       [ HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login( login login, string ReturnUrl = "")
        {
            string Message = "";
            using (MydataEntities dc = new MydataEntities())
            {
                var v = dc.Students.Where(a => a.E_Mail == login.E_Mail).FirstOrDefault();
                if (v != null)
                {
                    if (!v.E_MailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.E_Mail, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        Message = "Invalid credential provided";
                    }
                }
                else
                {
                    Message = "Invalid credential provided";
                }
            }
            ViewBag.Message = Message;
            return View();
        }



        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Student");
        }


        [NonAction]
        public bool IsExsist(string email)
        {
            using (MydataEntities dc = new MydataEntities()) {
                var v = dc.Students.Where(a => a.E_Mail == email).FirstOrDefault();

                return v != null;
            }
        }


        [NonAction]
        public void sendemailverifiction(string E_Mail, string ActivationCode)
        {

            var verifyURL = "/Student/VerifyAccount/" + ActivationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);
            var fromEmail = new MailAddress("abanoubreda68@outlook.com", "Dotnet Awesome");
            var toEmail = new MailAddress(E_Mail);
            var fromEmailPassword = "popo01282528521";
            string sbject = "Your account is Successfully Created";
            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
          " successfully created. Please click on the below link to verify your account" +
          " <br/><br/><a href='" + link + "'>" + link + "</a> ";

        var smtp = new SmtpClient
        {
            Host = "smtp.outlook.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = sbject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

    }
}
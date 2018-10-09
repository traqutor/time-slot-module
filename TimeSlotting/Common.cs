using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.ComponentModel;
using TimeSlotting.Models;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities.Logs;

namespace TimeSlotting
{
    [HandleError]
    public class Common
    {
        public static UserManager<User> GetUserManager()
        {
            var userManager = new UserManager<User>(new UserStore<User>(TimeSlottingDBContext.Create()));
            userManager.UserValidator = new UserValidator<User>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            return userManager;
        }

        public static WebUser GetUser(string id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.WebUsers
                          where m.ASPId == id
                          select m).ToList();

            var user = new WebUser();
            if (result.Count > 0)
            {
                user = result[0];
            }

            db.Dispose();

            return user;
        }

        public static int GetUserId(string id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.WebUsers
                          where m.ASPId == id
                          select m).ToList();

            var userId = 0;
            if (result.Count > 0)
            {
                userId = result[0].Id;
            }

            db.Dispose();

            return userId;
        }

        public static string GetUserRoleName(string id)
        {
            var db = new TimeSlottingDBContext();

            var name = GetRoleName(db.Users.Find(id).Roles.First().RoleId);

            db.Dispose();

            return name;
        }

        public static string GetFirstName(string id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.WebUsers
                          where m.ASPId == id
                          select m).ToList();

            var name = "";
            if (result.Count > 0)
            {
                name = result[0].FirstName;
            }

            db.Dispose();

            return name;
        }

        public static string GetUserName(int id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.WebUsers
                          where m.Id == id
                          select m).ToList();

            var name = "";
            if (result.Count > 0)
            {
                name = result[0].FirstName + " " + result[0].LastName;
            }

            db.Dispose();

            return name;
        }

        public static string GetRoleName(string id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.Roles
                          where m.Id == id
                          select m).ToList();

            var name = "";
            if (result.Count > 0)
            {
                name = result[0].Name;
            }

            db.Dispose();

            return name;
        }

        public static int GetCustomerId(string id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.WebUsers
                          where m.ASPId == id
                          select m).ToList();

            var cId = 0;
            if (result.Count > 0)
            {
                cId = (int)result[0].CustomerId;
            }

            db.Dispose();

            return cId;
        }

        public static string GetUserEmail(int id)
        {
            var db = new TimeSlottingDBContext();

            var result = (from m in db.Users
                          join u in db.WebUsers on m.Id equals u.ASPId
                          where u.Id == id
                          select m).ToList();

            var email = "";
            if (result.Count > 0)
            {
                email = result[0].Email;
            }

            db.Dispose();

            return email;
        }

        // EMAIL


        public static async Task<string> SendEmail(string to, string subject, string body, string attachFilePath, string url, string filename, int? userId, int? clientId, int? quoteId, int? jobId)
        {
            var result = "";
            var sendTo = to;

            MailMessage message = new MailMessage(TimeSlotting.Properties.Settings.Default.SMTP_Username, sendTo, subject, body);
            message.IsBodyHtml = true;

            if (attachFilePath != "")
            {
                Attachment file = new Attachment(attachFilePath);
                message.Attachments.Add(file);
            }

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                smtp.Host = TimeSlotting.Properties.Settings.Default.SMTP_Host;
                smtp.TargetName = $"STARTTLS/{TimeSlotting.Properties.Settings.Default.SMTP_Host}";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(TimeSlotting.Properties.Settings.Default.SMTP_Username, TimeSlotting.Properties.Settings.Default.SMTP_Password);
                smtp.Port = 587;
                Task.Run(() => smtp.SendAsync(message, new { S = subject, M = message, E = sendTo, U = userId, R = url, F = filename, A = attachFilePath }));

                result = "";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            //message.Dispose();
            return result;
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            dynamic state = e.UserState;

            if (state.M != null)
            {
                ((MailMessage)state.M).Dispose();
            }

            var log = new EmailLog();
            log.Recipient = state.E;
            log.Subject = state.S;
            log.WebUserId = null;


            if (state.U != null)
            {
                log.WebUserId = (int?)state.U;
            }
            if (state.A != null)
            {
                log.Attachment = state.A;
            }
            if (state.R != null)
            {
                log.URL = state.R;
            }
            if (state.F != null)
            {
                log.Filename = state.F;
            }

            log.SentDate = DateTime.Now;

            string error = "";
            if (e.Cancelled)
            {
                error = "Cancelled";
            }
            if (e.Error != null)
            {
                error = e.Error.Message;
            }

            log.SendError = error;

            TimeSlottingDBContext db = new TimeSlottingDBContext();
            db.EmailLogs.Add(log);
            db.SaveChanges();
            db.Dispose();
        }
    }
}
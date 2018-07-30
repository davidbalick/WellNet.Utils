using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace WellNet.Utils
{
    public static class EMailer
    {
        private static string _authUserName;
        private static string AuthUserName
        {
            set { _authUserName = value; }
            get
            {
                if (!string.IsNullOrEmpty(_authUserName))
                    return _authUserName;
                GetAuths();
                return _authUserName;
            }
        }
        private static string _authPassword;
        private static string AuthPassword
        {
            set { _authPassword = value; }
            get
            {
                if (!string.IsNullOrEmpty(_authPassword))
                    return _authPassword;
                GetAuths();
                return _authPassword;
            }
        }

        const string EmailErrorsTo = "david.balick@wellnet.com";
        const string FromAddress = "no-reply@wellnet.com";
        const string Host = "outlook.office365.com";
        const int Port = 587;

        private static void GetAuths()
        {
            var rawS = TripleDes.DecryptFileToString(@"\\192.168.11.10\shared\apps\common\resources\emailauth.dat");
            var dict = new Dictionary<string, string>();
            var lines = rawS.Replace("\r", string.Empty).Split('\n');
            var parts = lines[0].Split(',');
            dict[parts[0]] = parts[1];
            parts = lines[1].Split(',');
            dict[parts[0]] = parts[1];
            _authUserName = dict["AuthUserName"];
            _authPassword = dict["AuthPassword"];
        }

        public static string Email(string subject, string to, string message)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(FromAddress),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            msg.To.Add(new MailAddress(to));
            var client = new SmtpClient
            {
                Host = Host,
                Credentials = new System.Net.NetworkCredential(AuthUserName, AuthPassword),
                Port = Port,
                EnableSsl = true
            };
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public static string EmailError(string message)
        {
            var subject = string.Format("Error from: {0}", System.Reflection.Assembly.GetEntryAssembly().Location);
            var body = string.Format("<BR/>User: {0}<BR/>Exception: {1}", GetUserName(), message);
            return Email(subject, EmailErrorsTo, body);
        }

        private static string GetUserName()
        {
            return string.Format("{0}\\{1}",
                Environment.GetEnvironmentVariable("USERDOMAIN"), Environment.GetEnvironmentVariable("USERNAME"));
        }
    }
}

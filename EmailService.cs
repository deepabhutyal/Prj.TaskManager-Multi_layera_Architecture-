using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Prj.TaskManager.Sevice
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        string smtpServer;
        int port;
        string senderEmail;
        string password;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            smtpServer = _configuration["EmailSettings:SmtpServer"];
            port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            senderEmail = _configuration["EmailSettings:SenderEmail"];
            password = _configuration["EmailSettings:SenderPassword"];
        }
        //public async Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    //msg create garne
        //    var message = new MimeMessage();
        //    var fromMailBoxAddress = new MailboxAddress("Task Manager", senderEmail);
        //    message.From.Add(fromMailBoxAddress);

        //    var toMailboxAddress = new MailboxAddress("", toEmail);
        //    message.To.Add(toMailboxAddress);
        //    message.Subject = subject;
        //    var bodyBuilder = new BodyBuilder { HtmlBody = body };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync(smtpServer, port, false);
        //        await client.AuthenticateAsync(senderEmail, password);
        //        await client.SendAsync(message);
        //        await client.DisconnectAsync(true);
        //    }


        //}
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var smtpClient = new System.Net.Mail.SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),  // <-- fix here
                Credentials = new NetworkCredential(
               _configuration["EmailSettings:SenderEmail"],
               _configuration["EmailSettings:SenderPassword"]),
                EnableSsl = true
            };


            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SenderEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

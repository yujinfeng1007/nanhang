using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ZHXY.Common
{
    public static class MailHelper
    {
        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mail">邮件</param>
        /// <param name="password">密码/授权码</param>
        /// <param name="host">发件服务器</param>
        /// <param name="port">发件服务器端口号</param>
        /// <param name="enabledSsl">是否启用ssl</param>
        public static void Send(this MailMessage mail, string password, string host, int port = 0, bool enabledSsl = false)
        {
            var client = new SmtpClient(host, port);
            client.EnableSsl = enabledSsl;
            client.Credentials = new NetworkCredential(mail.From.Address, password);
            mail.Priority = MailPriority.High;
            mail.HeadersEncoding = mail.SubjectEncoding = mail.BodyEncoding = Encoding.UTF8;
            try
            {
                if (string.IsNullOrEmpty(mail.From.Address)) throw new ArgumentException("发件人邮箱不合法!");
                client.Send(mail);
            }
            finally
            {
                client.Dispose();
            }
        }

        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="from">发送人邮箱地址</param>
        /// <param name="password">发送人邮箱密码/授权码</param>
        /// <param name="to">接收人邮箱地址,如果是多个请用英文逗号进行分隔</param>
        /// <param name="cc">抄送人邮箱地址,如果是多个请用英文逗号进行分隔</param>
        /// <param name="bcc">密送人邮箱地址,如果是多个请用英文逗号进行分隔</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="attachments"></param>
        /// <param name="host">发件服务器地址</param>
        /// <param name="port">发件服务器端口</param>
        /// <param name="enabledSsl">是否启用ssl加密</param>
        /// <param name="isBodyHtml">邮件内容是否是html格式</param>
        public static void SendMail(string from, string password, string to, string cc, string bcc, string subject,
            string content, string attachments, string host, int port = 0, bool enabledSsl = false,
            bool isBodyHtml = true)
        {
            var mail = new MailMessage(from, to);
            if (!string.IsNullOrWhiteSpace(cc))
            {
                var arr = cc.Split(',', ';');
                foreach (var item in arr) mail.CC.Add(item);
            }

            if (!string.IsNullOrWhiteSpace(bcc))
            {
                var arr = bcc.Split(',');
                foreach (var item in arr) mail.Bcc.Add(item);
            }

            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = isBodyHtml;
            var tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp",
                Guid.NewGuid().ToString("N").ToUpper());
            Directory.CreateDirectory(tempDir);
            if (!string.IsNullOrWhiteSpace(attachments))
            {
                var arrs = attachments.Split(',', ';');
                foreach (var item in arrs)
                {
                    var index = item.LastIndexOf('/');
                    var fileName = item.Substring(index + 1);
                    FtpHelper.DownloadFile(item.Substring(1), tempDir);
                    var fs = new FileStream(Path.Combine(tempDir, fileName), FileMode.Open, FileAccess.Read);
                    mail.Attachments.Add(new Attachment(fs, fileName));
                }
            }

            mail.Send(password, host, port, enabledSsl);
        }
    }
}
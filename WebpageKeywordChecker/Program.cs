using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace WebpageKeywordChecker
{
    public class Program
    {
        static Logger logger = null;

        static void Main(string[] args)
        {
            logger = new Logger();
            Console.WriteLine(logger.Location);
            Find();
            Console.ReadLine();
        }

        public static string GetWebpage(string url)
        {
            string webpageData = String.Empty;
            HttpWebRequest req = null;
            HttpWebResponse res = null;

            try
            {
                req = WebRequest.Create(url) as HttpWebRequest;
                res = req.GetResponse() as HttpWebResponse;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Stream received = res.GetResponseStream();
                    StreamReader readStream = null;

                    if (res.CharacterSet == null)
                    {
                        readStream = new StreamReader(received);
                    }
                    else
                    {
                        readStream = new StreamReader(received, Encoding.GetEncoding(res.CharacterSet));
                    }

                    webpageData = readStream.ReadToEnd();
                }
            }
            catch (WebException webex)
            {
                // log this exception
                logger.WriteToLog(webex.ToString());
            }
            catch (Exception ex)
            {
                // log this exception
                logger.WriteToLog(ex.ToString());
            }
            finally
            {

            }

            return webpageData;
        }

        public static void Find()
        {
            string url = Properties.Settings.Default.UrlToCheck;
            string find = Properties.Settings.Default.TextToFind;
            string webpageData = GetWebpage(url);
            bool found = webpageData.Contains(find);
            //Console.WriteLine(found.ToString());
            if (true)
            {
                // send an email
                SendEmail();
            }
        }

        public static void SendEmail()
        {
            string from = Properties.Settings.Default.FromEmail;
            string to = Properties.Settings.Default.ToEmail;

            var fromAddress = new MailAddress(from, "Found");
            var toAddress = new MailAddress(to, "Chris");
            string fromPassword = Properties.Settings.Default.EmailPassword;
            const string subject = "Found your item";
            string body = String.Format("Found your thing at {0}", Properties.Settings.Default.UrlToCheck);

            var smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(from, "");
            smtpClient.Host = "smtp.live.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.SendCompleted += SmtpClientSendCompleted;
            var mailMessage = new MailMessage(from, to, subject, body);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                logger.WriteToLog(ex.ToString());
            }
        }

        private static void SmtpClientSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            logger.WriteToLog("Email sent!");
        }
    }
}

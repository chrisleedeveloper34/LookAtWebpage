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

        public void Find()
        {
            string url = Properties.Settings.Default.UrlToCheck;
            string find = Properties.Settings.Default.TextToFind;
            string webpageData = GetWebpage(url);
            bool found = webpageData.Contains(find);
            //Console.WriteLine(found.ToString());
            if (found)
            {
                // send an email

            }
        }

        public void SendEmail()
        {
            string from = Properties.Settings.Default.FromEmail;
            string to = Properties.Settings.Default.ToEmail;

            var fromAddress = new MailAddress(from, "Found");
            var toAddress = new MailAddress(to, "Chris");
            string fromPassword = Properties.Settings.Default.GmailPassword;
            const string subject = "Found";
            string body = String.Format("Found your thing at {0}", Properties.Settings.Default.UrlToCheck);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebpageKeywordChecker
{
    public class Program
    {
        static Logger logger = null;

        static void Main(string[] args)
        {
            logger = new Logger();
            Console.WriteLine(logger.Location);
            string url = Properties.Settings.Default.UrlToCheck;
            string find = Properties.Settings.Default.TextToFind;
            string webpageData = GetWebpage(url);
            bool found = webpageData.Contains(find);
            Console.WriteLine(found.ToString());
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

                throw new Exception("Testing");
            }
            catch (WebException webex)
            {
                // log this exception
            }
            catch (Exception ex)
            {
                // log this exception
                logger.WriteToLog(ex.Message);
            }
            finally
            {

            }

            return webpageData;
        }

        public static void WriteToFile()
        {

        }
    }
}

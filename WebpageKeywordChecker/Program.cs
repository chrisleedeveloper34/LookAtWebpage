using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebpageKeywordChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = Properties.Settings.Default.UrlToCheck;
            string webpageData = GetWebpage(url);

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
            }
                catch (Exception ex)
            {
                    // log this exception
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

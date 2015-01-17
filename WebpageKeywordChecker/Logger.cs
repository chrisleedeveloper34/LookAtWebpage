using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WebpageKeywordChecker
{
    public class Logger
    {
        Assembly currentAssem = Assembly.GetExecutingAssembly();
        string logName = String.Format("log_{0:yyyy-MM-dd_HH-mm-ss}.log", DateTime.Now);
        string logPath = null;

        public string Location { get { return currentAssem.Location; } set { ;} }

        public Logger()
        {
            if(!Directory.Exists(Properties.Settings.Default.LogFolder))
            {
                Directory.CreateDirectory(Properties.Settings.Default.LogFolder);
            }
            logPath = String.Format(@"{0}\{1}_{2}", Properties.Settings.Default.LogFolder, currentAssem.GetName().Name, logName);
            File.Open(logPath, FileMode.Create).Close();
        }

        public void WriteToLog(string txt)
        {
            using (FileStream fs = File.Open(logPath, FileMode.Open))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(txt);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}

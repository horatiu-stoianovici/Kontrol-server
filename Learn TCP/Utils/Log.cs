using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kontrol.Utils
{
    public class Log
    {
        private static volatile bool isWritingLog = false;

        public static void Debug(String tag, String text)
        {
            WriteLog("Debug", tag, text);
        }

        public static void Info(String tag, String text)
        {
            WriteLog("Info", tag, text);
        }

        public static void Error(String tag, String text)
        {
            WriteLog("ERROR", tag, text);
        }

        public static void Warning(String tag, String text)
        {
            WriteLog("Warning", tag, text);
        }

        private static void WriteLog(String warningLevel, String tag, String text)
        {
            while (isWritingLog) ;

            isWritingLog = true;
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine("[{3}][{0}]\t({1}):\t{2}", warningLevel, tag, text, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            }
            isWritingLog = false;
        }
    }
}
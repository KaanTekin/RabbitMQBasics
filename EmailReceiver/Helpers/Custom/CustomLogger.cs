using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailReceiver.Helpers.Custom
{
    public class CustomLogger
    {
        public static void WriteLog(string logMessage)
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Logs\";
            string fileName = $"log-{DateTime.Now.ToLongDateString().Replace(" ", "-")}" + ".txt";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            FileStream fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Append, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);

            sw.Write("\r\nLog Entry : ");
            sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            sw.WriteLine("  :");
            sw.WriteLine("  :{0}", logMessage);
            sw.WriteLine("-------------------------------");

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}

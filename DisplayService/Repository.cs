using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DisplayService
{
    public static class Repository
    {

        public static void logError(string msg)
        {
            string errorLogFilePath = ConfigurationManager.AppSettings["ErrorLog"]; ;
            var errorLog = new System.IO.StreamWriter(errorLogFilePath, true);
            errorLog.WriteLine(DateTime.Now.ToString() + " :  " + msg + "\n");
            errorLog.Close();
        }
    }
}

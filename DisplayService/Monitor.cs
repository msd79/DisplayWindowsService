using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace DisplayService
{
    public  class Monitor
    {

        private IHubProxy _hubproxy;
        
        public Monitor(IHubProxy proxy)
        {
            _hubproxy = proxy;
        }

        string ticketContent = ConfigurationManager.AppSettings["TicketContent"];

        public void WatchFileSystem(string path)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(path);
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;   
            fileSystemWatcher.EnableRaisingEvents = true;
        }


        //File system watcher event handler
        public void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string path = "c:\\code\\testingsservice.txt";
            try
            {
                string text = System.IO.File.ReadAllText(ticketContent);
                _hubproxy.Invoke("Send", text);
            }

            catch (Exception ex)
            {

                File.WriteAllText(path, ex.ToString());
            }

        }
    }
}

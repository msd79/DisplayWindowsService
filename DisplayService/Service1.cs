using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNet.SignalR.Client;
using System.IO;
using System.Threading;

namespace DisplayService
{

    public partial class Service1 : ServiceBase
    {

        public Service1()
        {
            InitializeComponent();
        }

        private string _path = ConfigurationManager.AppSettings["PathToMonitor"];
        private string _url = ConfigurationManager.AppSettings["DisplayUrl"];
        private HubConnection _hubConnection = null;
        private IHubProxy _displayHubProxy = null;
        private int retryCount = 0;
        public void InitialzeConnection()
        {
            if (_hubConnection != null)
            {
                // Clean up previous connection
                _hubConnection.Closed -= OnDisconnected;
            }

            if(retryCount < 6)
            {
                retryCount++;
                _hubConnection = new HubConnection(_url);
                _hubConnection.Closed += OnDisconnected;
                _displayHubProxy = _hubConnection.CreateHubProxy("DisplayHub");

                ConnectWithRetry();

            }
            else
            {
                Stop();
            }

        }

        public void OnDisconnected()
        {
            Repository.logError("Hub connection error occured");
            
            // Small delay before retrying connection
            Thread.Sleep(5000);

            // Need to recreate connection
            InitialzeConnection();
        }

        private void ConnectWithRetry()
        {
            // If this fails, the 'Closed' event (OnDisconnected) is fired
            var t = _hubConnection.Start();

            t.ContinueWith(task =>
            {
                if (!task.IsFaulted)
                {
                    var monitor = new Monitor(_displayHubProxy);
                    monitor.WatchFileSystem(_path);
                }
            }).Wait();
        }

        protected override void OnStart(string[] args)
        {


            InitialzeConnection();


        }

        protected override void OnStop()
        {
            _hubConnection.Stop();
        }
    }
}
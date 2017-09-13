using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayService
{
    class HubClient
    {
        private IHubProxy _hubproxy;

        public async Task RunAsynch(string url)
        {
            var connection = new HubConnection(url);
            _hubproxy = connection.CreateHubProxy("DisplayHub");
            await connection.Start();
        }

        public async Task InvokeRefresh()
        {
            await _hubproxy.Invoke("Send", "Hellow from the send function");
        }
    }
}

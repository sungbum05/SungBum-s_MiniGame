using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FreeNet
{
    /// <summary>
	/// Endpoint정보를 받아서 서버에 접속한다.
	/// 접속하려는 서버 하나당 인스턴스 한개씩 생성하여 사용하면 된다.
	/// </summary>
    public class CConnector
    {
        public delegate void ConnectedHandler(CUserToken token);
        public ConnectedHandler connected_callback { get; set; }

        Socket client;

        CNetworkService newwork_service;

        public CConnector(CNetworkService network_wervice)
        {
            this.newwork_service = network_wervice;
            this.connected_callback = null;
        }

        public void connect(IPEndPoint remote_endpoint)
        {
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs event_arg = new SocketAsyncEventArgs();
            event_arg.Completed += on_connect_completed;
            event_arg.RemoteEndPoint = remote_endpoint;
            bool pending = this.client.ConnectAsync(event_arg);
            if(!pending)
            {
                on_connect_completed(null, event_arg);
            }
        }

        void on_connect_completed(object sender, SocketAsyncEventArgs e) 
        {
            if(e.SocketError== SocketError.Success)
            {
                CUserToken token = new CUserToken();

                this.newwork_service.on_connect_completed(this.client, token);

                if(this.connected_callback != null)
                {
                    this.connected_callback(token);
                }
            }

            else
            {
                Console.WriteLine(string.Format("Failed to connect. {0}", e.SocketError));
            }
        }
    }
}

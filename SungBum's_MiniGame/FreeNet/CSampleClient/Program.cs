using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSampleClient
{
    using FreeNet;
    using GameServer;
    using System.Net;

    internal class Program
    {
        static List<IPeer> game_servers = new List<IPeer>();

        static void Main(string[] args)
        {
            CPacketBufferManager.initialize(2000);
            CNetworkService service = new CNetworkService();

            CConnector connector = new CConnector(service);

            connector.connected_callback += on_connected_gameserver;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7979);
            connector.connect(endpoint);

            while(true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if(line == "q")
                {
                    break;
                }

                CPacket msg = CPacket.create((short)PROTOCOL.CHAT_MSG_REQ);
                msg.push(line);
                game_servers[0].send(msg);
            }

            ((CRemoteServerPeer)game_servers[0]).token.disconnect();
            Console.ReadKey();
        }

        static void on_connected_gameserver(CUserToken server_token)
        {
            lock(game_servers) 
            {
                IPeer server = new CRemoteServerPeer(server_token);
                game_servers.Add(server);
                Console.WriteLine("Connected");
            }
        }
    }
}

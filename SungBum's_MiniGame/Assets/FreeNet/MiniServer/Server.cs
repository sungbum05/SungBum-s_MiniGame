using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml.Serialization;
using TMPro;

public class Server : MonoBehaviour
{
    public TMP_InputField PortInput;

    public List<ServerClient> Clients;
    public List<ServerClient> DisconnectList;

    TcpListener TcpServer;
    bool ServerStarted;

    public void ServerCreate()
    {
        Clients = new List<ServerClient>();
        DisconnectList = new List<ServerClient>();

        try
        {
            int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);
            TcpServer = new TcpListener(IPAddress.Any, port);
            TcpServer.Start();

            StartListening();
            ServerStarted = true;
            Chat.Instance.ShowMessage($"서버가 포트번호:{port}에서 시작하였습니다.");
        }
        catch(Exception e )
        {
            Chat.Instance.ShowMessage($"Socket Error: {e.Message}");
        }
    }

    private void Update()
    {
        if (!ServerStarted) return;

        foreach(ServerClient C in Clients)
        {
            //한 개 클라이언트가 서버에서 연결이 끊겼을 때
            if(!IsConnected(C.Tcp))
            {
                C.Tcp.Close();
                DisconnectList.Add(C);
                continue;
            }

            // 계속 연결 되있는 경우
            else
            {
                NetworkStream S = C.Tcp.GetStream();
                if(S.DataAvailable)
                {
                    string Data = new StreamReader(S, true).ReadLine();
                    if (Data != null)
                        OnIncomingData(C, Data);
                        
                }
            }

            //연결 해제된 클라이언트 리스트 처리
            for (int i = 0; i < DisconnectList.Count - 1; i++)
            {
                Brodcast($"{DisconnectList[1].ClientName} 연결이 끊어졌습니다.", Clients);

                Clients.Remove(DisconnectList[i]);
                DisconnectList.RemoveAt(i);
            }
        }
    }

    bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    void StartListening()
    {
        TcpServer.BeginAcceptTcpClient(AcceptTcpClient, TcpServer);
    }

    void AcceptTcpClient(IAsyncResult Ar)
    {
        TcpListener listener = (TcpListener)Ar.AsyncState;
        Clients.Add(new ServerClient(listener.EndAcceptTcpClient(Ar)));
        StartListening();

        //브로드 캐스트 함수 사용하여 클라이언트 연결 메세지 보냄
        Brodcast("%NAME", new List<ServerClient>() { Clients[Clients.Count - 1] });
    }

    void OnIncomingData(ServerClient C, string Data)
    {
        if(Data.Contains("&NAME"))
        {
            C.ClientName = Data.Split('|')[1];
            Brodcast($"{C.ClientName}이 연결되었습니다.", Clients);
            return;
        }

        Brodcast($"{C.ClientName} : {Data} ㅋㅋ", Clients);
    }

    void Brodcast(string Data, List<ServerClient> Clients)
    {
        foreach(var C in Clients)
        {
            try
            {
                StreamWriter Writer = new StreamWriter(C.Tcp.GetStream());
                Writer.WriteLine(Data);
                Writer.Flush();
            }
            catch(Exception e)
            {
                Chat.Instance.ShowMessage($"쓰기 에러 : {e.Message}를 클라이언트에게 {C.ClientName}");
            }
        }
    }
}

public class ServerClient
{
    public TcpClient Tcp;
    public string ClientName;

    public ServerClient(TcpClient clientSocket)
    {
        ClientName = "Guest";
        Tcp = clientSocket;
    }
}

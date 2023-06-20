using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TypeServer : MonoBehaviour
{
    public InputField PortInput;

    public int MaxPlayerCount = 2;
    public List<TypeServerClient> Clients;
    public List<TypeServerClient> DisconnectList;

    TcpListener TcpServer;
    bool ServerStarted;

    public TypeClient Client;

    public void ServerCreate()
    {
        Clients = new List<TypeServerClient>();
        DisconnectList = new List<TypeServerClient>();

        try
        {
            int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);
            TcpServer = new TcpListener(IPAddress.Any, port);
            TcpServer.Start();

            StartListening();
            ServerStarted = true;
            Debug.Log($"서버가 포트번호:{port}에서 시작하였습니다.");

            Client.IsHost = true;
            Client.ConnectToServer();
        }
        catch (Exception e)
        {
            Debug.Log($"Socket Error: {e.Message}");
        }
    }

    private void Update()
    {
        if (!ServerStarted) return;

        foreach (TypeServerClient C in Clients)
        {
            //한 개 클라이언트가 서버에서 연결이 끊겼을 때
            if (!IsConnected(C.Tcp))
            {
                C.Tcp.Close();
                DisconnectList.Add(C);
                continue;
            }

            // 계속 연결 되있는 경우
            else
            {
                NetworkStream S = C.Tcp.GetStream();
                if (S.DataAvailable)
                {
                    string Data = new StreamReader(S, true).ReadLine();
                    if (Data != null)
                        OnIncomingData(C, Data);

                }
            }

            //연결 해제된 클라이언트 리스트 처리
            for (int i = 0; i < DisconnectList.Count - 1; i++)
            {
                BrodcastAll($"{DisconnectList[1].ClientName} 연결이 끊어졌습니다.", Clients);

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
        Clients.Add(new TypeServerClient(listener.EndAcceptTcpClient(Ar)));
        Debug.Log("서버 수락");

        StartListening();

        //브로드 캐스트 함수 사용하여 클라이언트 연결 메세지 보냄
        BrodcastAll("%NAME", new List<TypeServerClient>() { Clients[Clients.Count - 1] });
        BrodcastAll($"%USER|{Clients.Count}", Clients);
    }

    void OnIncomingData(TypeServerClient C, string Data)
    {
        Debug.Log("SeverIncome" + C.ClientName + "|" + Data);

        if (Data.Contains("&NAME"))
        {
            C.ClientName = Data.Split('|')[1];
            BrodcastAll($"{C.ClientName}이 연결되었습니다.", Clients);
            return;
        }

        else if (Data.Contains("&ANSWER"))
        {
            Debug.Log("OtherCheack");
            BrodcastOther(C, $"%OTHER|{Data.Split('|')[1]}", Clients);
        }

        BrodcastAll($"{C.ClientName} : {Data}", Clients);
    }

    void BrodcastAll(string Data, List<TypeServerClient> Clients)
    {
        foreach (var C in Clients)
        {
            try
            {
                StreamWriter Writer = new StreamWriter(C.Tcp.GetStream());
                Writer.WriteLine(Data);
                Writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log($"쓰기 에러 : {e.Message}를 클라이언트에게 {C.ClientName}");
            }
        }
    }

    void BrodcastOther(TypeServerClient Owner, string Data, List<TypeServerClient> Clients)
    {
        foreach (var C in Clients)
        {
            try
            {
                if (C.ClientName != Owner.ClientName)
                {
                    Debug.Log($"Owner:{Owner.ClientName}\nOther:{C.ClientName}\nData:{Data}");
                    StreamWriter Writer = new StreamWriter(C.Tcp.GetStream());
                    Writer.WriteLine(Data);
                    Writer.Flush();
                }
            }
            catch (Exception e)
            {
                Debug.Log($"쓰기 에러 : {e.Message}를 클라이언트에게 {C.ClientName}");
            }
        }
    }

    [System.Serializable]
    public class TypeServerClient
    {
        public TcpClient Tcp;
        public string ClientName;

        public TypeServerClient(TcpClient clientSocket)
        {
            ClientName = "Guest";
            Tcp = clientSocket;
        }
    }
}

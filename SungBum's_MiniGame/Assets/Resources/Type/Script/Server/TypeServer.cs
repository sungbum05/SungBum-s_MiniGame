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
            Debug.Log($"������ ��Ʈ��ȣ:{port}���� �����Ͽ����ϴ�.");

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
            //�� �� Ŭ���̾�Ʈ�� �������� ������ ������ ��
            if (!IsConnected(C.Tcp))
            {
                C.Tcp.Close();
                DisconnectList.Add(C);
                continue;
            }

            // ��� ���� ���ִ� ���
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

            //���� ������ Ŭ���̾�Ʈ ����Ʈ ó��
            for (int i = 0; i < DisconnectList.Count - 1; i++)
            {
                BrodcastAll($"{DisconnectList[1].ClientName} ������ ���������ϴ�.", Clients);

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
        Debug.Log("���� ����");

        StartListening();

        //��ε� ĳ��Ʈ �Լ� ����Ͽ� Ŭ���̾�Ʈ ���� �޼��� ����
        BrodcastAll("%NAME", new List<TypeServerClient>() { Clients[Clients.Count - 1] });
        BrodcastAll($"%USER|{Clients.Count}", Clients);
    }

    void OnIncomingData(TypeServerClient C, string Data)
    {
        Debug.Log("SeverIncome" + C.ClientName + "|" + Data);

        if (Data.Contains("&NAME"))
        {
            C.ClientName = Data.Split('|')[1];
            BrodcastAll($"{C.ClientName}�� ����Ǿ����ϴ�.", Clients);
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
                Debug.Log($"���� ���� : {e.Message}�� Ŭ���̾�Ʈ���� {C.ClientName}");
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
                Debug.Log($"���� ���� : {e.Message}�� Ŭ���̾�Ʈ���� {C.ClientName}");
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

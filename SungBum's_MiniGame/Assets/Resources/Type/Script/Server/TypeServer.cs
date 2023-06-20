using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public enum SendType
{
    All,
    Other,
}

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
            TypeChat.Instance.ShowMessage($"������ ��Ʈ��ȣ:{port}���� �����Ͽ����ϴ�.");

            Client.IsHost = true;
            Client.ConnectToServer();
        }
        catch(Exception e)
        {
            TypeChat.Instance.ShowMessage($"Socket Error: {e.Message}");
        }
    }

    private void Update()
    {
        if (!ServerStarted) return;

        foreach(TypeServerClient C in Clients)
        {
            //�� �� Ŭ���̾�Ʈ�� �������� ������ ������ ��
            if(!IsConnected(C.Tcp))
            {
                C.Tcp.Close();
                DisconnectList.Add(C);
                continue;
            }

            // ��� ���� ���ִ� ���
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

            //���� ������ Ŭ���̾�Ʈ ����Ʈ ó��
            for (int i = 0; i < DisconnectList.Count - 1; i++)
            {
                Brodcast($"{DisconnectList[1].ClientName} ������ ���������ϴ�.", Clients, SendType.All);

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
        Brodcast("%NAME", new List<TypeServerClient>() { Clients[Clients.Count - 1] }, SendType.All);
        Brodcast($"%USER|{Clients.Count}", Clients, SendType.All);
    }

    void OnIncomingData(TypeServerClient C, string Data)
    {
        if (Data.Contains("&NAME"))
        {
            C.ClientName = Data.Split('|')[1];
            Brodcast($"{C.ClientName}�� ����Ǿ����ϴ�.", Clients, SendType.All);
            return;
        }

        else if(Data.Contains("&ANSWER"))
        {
            Debug.Log("OtherCheack");
            Brodcast($"%OTHER|{Data}", Clients, SendType.Other);
        }

        Brodcast($"{C.ClientName} : {Data}", Clients, SendType.All);
    }

    void Brodcast(string Data, List<TypeServerClient> Clients, SendType Type)
    {
        if (Type == SendType.All)
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
                    TypeChat.Instance.ShowMessage($"���� ���� : {e.Message}�� Ŭ���̾�Ʈ���� {C.ClientName}");
                }
            }
        }

        else if(Type == SendType.Other)
        {
            if(Client.IsHost == true)
            {
                StreamWriter Writer = new StreamWriter(Clients[1].Tcp.GetStream());
                Writer.WriteLine(Data);
                Writer.Flush();
            }

            else
            {
                StreamWriter Writer = new StreamWriter(Clients[0].Tcp.GetStream());
                Writer.WriteLine(Data);
                Writer.Flush();
            }
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

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
            Chat.Instance.ShowMessage($"������ ��Ʈ��ȣ:{port}���� �����Ͽ����ϴ�.");
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
                Brodcast($"{DisconnectList[1].ClientName} ������ ���������ϴ�.", Clients);

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

        //��ε� ĳ��Ʈ �Լ� ����Ͽ� Ŭ���̾�Ʈ ���� �޼��� ����
        Brodcast("%NAME", new List<ServerClient>() { Clients[Clients.Count - 1] });
    }

    void OnIncomingData(ServerClient C, string Data)
    {
        if(Data.Contains("&NAME"))
        {
            C.ClientName = Data.Split('|')[1];
            Brodcast($"{C.ClientName}�� ����Ǿ����ϴ�.", Clients);
            return;
        }

        Brodcast($"{C.ClientName} : {Data} ����", Clients);
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
                Chat.Instance.ShowMessage($"���� ���� : {e.Message}�� Ŭ���̾�Ʈ���� {C.ClientName}");
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

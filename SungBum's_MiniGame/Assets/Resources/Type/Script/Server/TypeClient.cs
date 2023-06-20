using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;

public class TypeClient : MonoBehaviour
{
    public InputField IPInput, PortInput, NickInput;
    string ClientName;

    bool SocketReady;
    TcpClient Socket;
    NetworkStream Stream;
    StreamWriter Writer;
    StreamReader Reader;

    public GameObject StartPanel;
    public GameObject WaitPanel;

    [Header("Server Info")]
    public bool IsHost;
    public int MaxServerUser = 2;
    public int ServerUser = 0;

    public void ConnectToServer()
    {
        if (SocketReady) return;

        string Ip = IPInput.text == "" ? "127.0.0.1" : IPInput.text;
        int Port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

        try
        {
            Socket = new TcpClient(Ip, Port);
            Debug.Log("서버 접속");
            Stream = Socket.GetStream();
            Writer = new StreamWriter(Stream);
            Reader = new StreamReader(Stream);
            SocketReady = true;
        }
        catch (Exception e) 
        {
            TypeChat.Instance.ShowMessage($"소켓 에러 : {e.Message}");
        }
    }

    private void Update()
    {
        if(SocketReady && Stream.DataAvailable)
        {
            string Data = Reader.ReadLine();
            if (Data != null)
                OnIncomingData(Data);
        }

        //서버 유저 정보 받아옴(임시)
        if(ServerUser > 0 && ServerUser < MaxServerUser)
        {
            StartPanel.SetActive(false);
            WaitPanel.SetActive(true);
        }

        else if(ServerUser > 0 && ServerUser >= MaxServerUser)
        {
            StartPanel.SetActive(false);
            WaitPanel.SetActive(false);
        }
    }

    void OnIncomingData(string Data)
    {
        if(Data == "%NAME")
        {
            ClientName = NickInput.text == "" ? "Guest" + UnityEngine.Random.Range(1, 10000) : NickInput.text;
            Send($"&NAME|{ClientName}");
            return;
        }
        else if (Data.Split('|')[0] == "%USER")
            ServerUser = int.Parse(Data.Split('|')[1]);
        else if (Data.Split('|')[0] == "%OTHER")
            TypeServerManager.Instance.MultiInput.OtherAnswer(Data.Split('|')[1]);

        TypeChat.Instance.ShowMessage(Data);
    }

    public void Send(string Data)
    {
        if(!SocketReady) return;

        Writer.WriteLine(Data);
        Writer.Flush();
    }

    public void OnSendButton(InputField SendInput)
    {
#if(UNITY_EDITOR || UNITY_STANDALONE)
        if (!Input.GetButtonDown("Submit")) return;
        SendInput.ActivateInputField();
#endif
        if (SendInput.text.Trim() == "") return;

        string Message = SendInput.text;
        Send(Message);
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!SocketReady) return;

        Writer.Close();
        Reader.Close();
        Socket.Close();
        SocketReady = false;
    }
}

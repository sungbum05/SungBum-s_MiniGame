using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;

public class Client : MonoBehaviour
{
    public TMP_InputField IPInput, PortInput, NickInput;
    string ClientName;

    bool SocketReady;
    TcpClient Socket;
    NetworkStream Stream;
    StreamWriter Writer;
    StreamReader Reader;

    public void ConnectToServer()
    {
        if (SocketReady) return;

        string Ip = IPInput.text == "" ? "127.0.0.1" : IPInput.text;
        int Port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

        try
        {
            Socket = new TcpClient(Ip, Port);
            Stream = Socket.GetStream();
            Writer = new StreamWriter(Stream);
            Reader = new StreamReader(Stream);
            SocketReady = true;
        }
        catch (Exception e) 
        {
            Chat.Instance.ShowMessage($"소켓 에러 : {e.Message}");
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
    }

    void OnIncomingData(string Data)
    {
        if(Data == "%NAME")
        {
            ClientName = NickInput.text == "" ? "Guest" + UnityEngine.Random.Range(1, 10000) : NickInput.text;
            Send($"&NAME|{ClientName}");
            return;
        }

        Chat.Instance.ShowMessage(Data);
    }

    void Send(string Data)
    {
        if(!SocketReady) return;

        Writer.WriteLine(Data);
        Writer.Flush();
    }

    public void OnSendButton(TMP_InputField SendInput)
    {
#if(UNITY_EDITOR || UNITY_STANDALONE)
        if (!Input.GetButtonDown("Submit")) return;
        SendInput.ActivateInputField();
#endif
        if (SendInput.text.Trim() == "") return;

        string Message = SendInput.text;
        SendInput.text = "";
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

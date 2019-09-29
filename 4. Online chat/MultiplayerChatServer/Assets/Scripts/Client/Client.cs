using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    [SerializeField] private string _name;
    private bool _socketReady;
    private TcpClient _socket;
    private NetworkStream _stream;
    private StreamWriter _streamWriter;
    private StreamReader _streamReader;

    public GameObject ChatContainer;
    public GameObject MessagePrefab;
    
    public void ConnectToServer()
    {
        if(_socketReady)
            return;

        string host = "127.0.0.1";
        int port = 6321;

        var h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (h != string.Empty)
            host = h;
        int p;
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
            port = 0;
            

        Debug.Log( host+" - "+port);
        //  создаем socket
        try
        {
            _socket = new TcpClient(host,port);

            _stream = _socket.GetStream();
            _streamWriter = new StreamWriter(_stream);
            _streamReader = new StreamReader(_stream);
            _socketReady = true;
        }
        catch (Exception e)
        {
            Debug.LogError("CLIENT = "+e.Message);
        }

    }

    private void Update()
    {
        
        if (_socketReady)
        {
            if (_stream.DataAvailable)
            {
                Debug.Log("DataAvailable");
                string data = _streamReader.ReadLine();
                if (data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
    }

    private void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            Send("&NAME|"+_name);
            return;
        }
        
        var message = Instantiate(MessagePrefab, ChatContainer.transform);
        message.GetComponentInChildren<Text>().text = data;
    }

    private void Send(string data)
    {
        if(!_socketReady)
            return;
        
        _streamWriter.WriteLine(data);
        _streamWriter.Flush();
    }

    public void OnSendButton()
    {
        string message = GameObject.Find("SendInput").GetComponent<InputField>().text;
        Send(message);
    }

    private void CloseSocket()
    {
        if(!_socketReady)
            return;    
        
        _streamWriter.Close();
        _streamReader.Close();
        _socket.Close();
        _socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }
}

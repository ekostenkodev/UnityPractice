using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class Client : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private string _ip = "127.0.0.1";
    [SerializeField] private int _port = 6321;

    private bool _socketReady;
    private TcpClient _socket;
    private NetworkStream _stream;
    private StreamWriter _streamWriter;

    #region MonoBehaviour
    private void Start()
    {
        ConnectToServer();
    }

    private void OnDisable()
    {
        if(_socketReady)
        {
            _stream.Close();
            _streamWriter.Close();
            _socketReady = false;
            _socket.Close();
        }
    }
    #endregion

    private void ConnectToServer()
    {
        if (_socketReady)
            return;

        try
        {
            _socket = new TcpClient(_ip, _port);

            _stream = _socket.GetStream();
            _streamWriter = new StreamWriter(_stream);
            _socketReady = true;
        }
        catch (Exception e)
        {
            Debug.LogError("CLIENT : " + e.Message);
        }

    }

    public void SendMessageToServer(string message)
    {
        if (!_socketReady)
            return;

        _streamWriter.WriteLine(message);
        _streamWriter.Flush();
    }
}

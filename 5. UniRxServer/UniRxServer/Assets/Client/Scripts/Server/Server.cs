using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UniRx;
using System.Text;

public class Server : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private int _port = 6321;
    private TcpListener _server;

    public ISubject<string> Data;

    #region MonoBehaviour
    private void Awake()
    {
        Data = new Subject<string>();

        _server = new TcpListener(IPAddress.Any, _port);
        _server.Start();
        StartAcceptClient();
    }
    #endregion

    private void StartAcceptClient()
    {
        _server.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), _server);
    }

    private void AcceptTcpClientCallback(IAsyncResult asyncResult)
    {
        TcpListener tcpListener = (TcpListener)asyncResult.AsyncState;

        if (tcpListener != null)
        {
            var client = tcpListener.EndAcceptTcpClient(asyncResult);
            if (client.Connected)
            {
                BeginRead(client.Client, new byte[1024]);
                StartAcceptClient();
            }
        }
    }

    private void BeginRead(Socket socket, byte[] buffer)
    {

        AsyncCallback callback = asyncResult =>
        {
            try
            {
                Socket tcpSocket = (Socket)asyncResult.AsyncState;

                int bytesRead = tcpSocket.EndReceive(asyncResult);

                if (bytesRead > 0)
                {
                    var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Data.OnNext(data);
                }
                else
                {
                    return;
                }

                BeginRead(tcpSocket, buffer);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        };

        try
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, socket);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}


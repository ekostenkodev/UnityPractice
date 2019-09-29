using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    private List<ServerClient> _clients;
    private List<ServerClient> _disconnections;
    
    [SerializeField] private int _port = 6321;
    private TcpListener _server;
    private bool _serverStarted;

    private void Start()
    {
        _clients = new List<ServerClient>();
        _disconnections = new List<ServerClient>();

        try
        {
            _server = new TcpListener(IPAddress.Any,_port);
            _server.Start();

            StartListening();
            _serverStarted = true;
            
            Debug.Log("Сервер начал работу на "+_port);
        }
        catch (Exception e)
        {
            Debug.LogError("OOPS : "+e.Message);
            throw;
        }
    }

    private void StartListening()
    {
        _server.BeginAcceptTcpClient(AcceptTcpClient,_server);
    }

    private void AcceptTcpClient(IAsyncResult result)
    {

        TcpListener listener = (TcpListener)result.AsyncState;
        var res = listener.EndAcceptTcpClient(result);
        
        var client = new ServerClient(res);

        _clients.Add(client);
        Broadcast("%NAME",new List<ServerClient>(){_clients[_clients.Count-1]});
        
        //Broadcast(_clients[_clients.Count-1].Name +" has connected ds",_clients);
        
        StartListening();
        
    }

    private void Broadcast(string data, List<ServerClient> clients)
    {
        foreach (var client in clients)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.Tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.LogError("Broadcast : " + e.Message+" to client : "+client);
            }
        }
    }

    private void Update()
    {
        if(!_serverStarted)
            return;

        foreach (var client in _clients)
        {
            if (!IsConnected(client.Tcp))
            {
                client.Tcp.Close();
                _disconnections.Add(client);
            }
            else
            {
                NetworkStream stream = client.Tcp.GetStream();
                if (stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream,true);
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        OnIncomingData(client, data);
                    }
                }
            }
        }

        for (int i = 0; i < _disconnections.Count-1; i++)
        {
            Broadcast(_disconnections[i].Name + " вышел",_clients);
            _clients.Remove(_disconnections[i]);
            _disconnections.RemoveAt(i);
        }
    }

    private void OnIncomingData(ServerClient client, string data)
    {
        if (data.Contains("&NAME"))
        {
            client.Name = data.Split('|')[1];
            Broadcast(client.Name + "  присоединился",_clients);
            return;
        }
        
        Broadcast(client.Name + " : " + data,_clients);
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            // WHAT?
            if (client != null && client.Client != null && client.Client.Connected)
            {
                if (client.Client.Poll(0, SelectMode.SelectRead))
                {
                    return client.Client.Receive(new byte[1], SocketFlags.Peek) != 0;
                }

                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("OOPS CLIENT : "+e.Message);
        }

        return false;
    }
}

public class ServerClient
{
    public TcpClient Tcp { get; private set; }
    public string Name { get; set; }

    public ServerClient(TcpClient tcp)
    {
        Tcp = tcp;
        Name = "Guest";
    }
}

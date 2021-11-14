using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab01_Server;

public class Server
{
    private readonly int _port;
    private List<IPEndPoint> _clients;
    private Socket _socket;

    private readonly int _number;
    private IPEndPoint _activeClient;

    public Server(int port)
    {
        _port = port;
            
        CreateClients();
        _number = new Random().Next(1, 1000);
    }
    
    public void Init()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _activeClient = _clients.First();
    }

    public void Start()
    {
        try
        {
            Init();

            Task listeningTask = new Task(Listen);
            listeningTask.Start();

            while (true) { }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void SendMessage(string message)
    {
        foreach (var ipEndPoint in _clients)
        {
            var data = Encoding.Unicode.GetBytes(message);
            _socket.SendTo(data, ipEndPoint);
            Console.WriteLine($"Sent message to client {ipEndPoint.Address}:{ipEndPoint.Port}");
        }
    }
    
    public void SendMessage(IPEndPoint clientIp, string message)
    {
        foreach (var ipEndPoint in _clients)
        {
            if (ipEndPoint.Equals(clientIp))
            {
                continue;
            }
            var data = Encoding.Unicode.GetBytes(message);
            _socket.SendTo(data, ipEndPoint);
            Console.WriteLine($"Sent message to client {ipEndPoint.Address}:{ipEndPoint.Port}");
        }
    }

    public void SendMessageToClient(IPEndPoint clientIp, string message)
    {
        var data = Encoding.Unicode.GetBytes(message);
        _socket.SendTo(data, clientIp);
        Console.WriteLine($"Sent message to client {clientIp.Address}:{clientIp.Port}");
    }

    public void Listen()
    {
        try
        {
            // Port to listen on
            IPEndPoint localIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            _socket.Bind(localIp);

            while (true)
            {
                StringBuilder builder = new StringBuilder();

                int bytesCount = 0; // amount of received bytes
                byte[] data = new byte[256]; //buffer for received data

                //Address to receive from
                EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                do
                {
                    bytesCount = _socket.ReceiveFrom(data, ref remoteIp);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytesCount));
                }
                while (_socket.Available > 0);
                IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                if (remoteFullIp.Equals(_activeClient))
                {
                    SendMessage(remoteFullIp, $"{remoteFullIp.Address}:{remoteFullIp.Port} - {builder}");
                    CalculateResult(Convert.ToInt32(builder.ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void CreateClients()
    {
        _clients = new List<IPEndPoint>();
        var _clientIp = IPAddress.Parse("127.0.0.1");
        _clients.Add(new IPEndPoint(_clientIp, 56502));
        _clients.Add(new IPEndPoint(_clientIp, 56503));
    }

    private void CalculateResult(int number)
    {
        CalculateNum(number);
        CalculateNextClient();
    }

    private void CalculateNum(int num)
    {
        if (num == _number)
        {
            SendMessage("It`s true");
        }
        else if (num > _number)
        {
            SendMessage("<");
        }
        else if (num < _number)
        {
            SendMessage(">");
        }
    }

    private void CalculateNextClient()
    {
        bool isActivePlayer = false;
        foreach (var clientIp in _clients)
        {
            if (isActivePlayer)
            {
                _activeClient = clientIp;
                SendMessageToClient(_activeClient, "You turn");
                break;
            }
            
            if (clientIp.Port == _activeClient.Port)
            {
                isActivePlayer = true;
                if (_clients.Last().Equals(clientIp))
                {
                    _activeClient = _clients.First();
                    SendMessageToClient(_activeClient, "You turn");
                    break;
                }
            }
        }
    }
}
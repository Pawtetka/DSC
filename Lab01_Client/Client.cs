using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab01_Client
{
    public class Client
    {
        private readonly int _port;
        private readonly int _serverPort;
        private Socket _socket;

        public Client(int port, int serverPort)
        {
            _port = port;
            _serverPort = serverPort;
        }

        public void Start()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                var listeningTask = new Task(Listen);
                listeningTask.Start();
                while (true)
                {
                    string message = Console.ReadLine();
                    Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        
        private void Listen()
        {
            try
            {
                IPEndPoint localIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
                _socket.Bind(localIp);
 
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    byte[] data = new byte[256]; 
 
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
 
                    do
                    {
                        var bytes = _socket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_socket.Available > 0);
                    
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;
                    
                    Console.WriteLine($"{remoteFullIp.Address}:{remoteFullIp.Port} - {builder}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        public void Send(string message)
        {
            var data = Encoding.Unicode.GetBytes(message);
            EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _serverPort);
            _socket.SendTo(data, remotePoint);
        }

        private void Close()
        {
            if (_socket == null) return;
            
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
        }
    }
}
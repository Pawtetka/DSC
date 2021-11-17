using System;
using System.Net.Http;
using System.Threading.Tasks;
using gRPC_Server;
using Grpc.Net.Client;

namespace ClientApp
{
    class Program
    {
        static private int clientId = 0;
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions()
            {
                HttpHandler = GetHttpClientHandler()
            });
            
            // создаем клиента
            var client = new Sender.SenderClient(channel);
            
            Console.Write("Give me your user id: ");
            clientId = Convert.ToInt32(Console.ReadLine());

            var timer = new Timer(OnTimerEnd, null, 0, 5000);

            while (true)
            {
            }

            async void OnTimerEnd(object source)
            {
                var reply = await client.SendMessageAsync(new MessageRequest { ClientId = clientId } );
                if (!reply.Message.Equals(""))
                {
                    Console.WriteLine(reply.Message);
                }
            }
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return httpHandler;
        }
    }
}
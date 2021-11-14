using System;

namespace Lab01_Server
{
    class Program
    {
        public const int localPort = 56501;

        static void Main(string[] args)
        {
            var server = new Server(localPort);

            server.Start();
        }
    }
}
using System;
using Lab01_Client;

Console.Write("Enter the port: ");
var port = int.Parse(Console.ReadLine() ?? "56502");
var serverPort = 56501;

var client = new Client(port, serverPort);
client.Start();
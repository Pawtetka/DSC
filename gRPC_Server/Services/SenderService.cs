using System.Security.Authentication;
using Grpc.Core;
using gRPC_Server;

namespace gRPC_Server.Services;

public class SenderService : Sender.SenderBase
{
    private readonly ILogger<SenderService> _logger;

    public SenderService(ILogger<SenderService> logger)
    {
        _logger = logger;
    }

    public override Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
    {
        string message = "";
        if (IsClientAvailableInList(request.ClientId, "./clients.txt"))
        {
            message = "Hello client number " + request.ClientId;
        }

        return Task.FromResult(new MessageReply()
        {
            Message = message
        });
    }
    
    private bool IsClientAvailableInList(int clientId, string filename)
    {
        var lines = File.ReadLines(filename).ToList();
        foreach (var line in lines)
        {
            if (Convert.ToInt32(line) == clientId)
            {
                return true;
            }
        }

        return false;
    }
}
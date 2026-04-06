using Google.Protobuf.WellKnownTypes;
using gRoom.gRPC.Messages;
using gRoom.gRPC.Utils;
using Grpc.Core;

namespace gRoom.gRPC.Services;

public class GroomService : Groom.GroomBase
{
    private readonly ILogger<GroomService> _logger;

    public GroomService(ILogger<GroomService> logger)
    {
        _logger = logger;
    }

    public override async Task<RoomRegistrationResponse> RegisterToRoom(
        RoomRegistrationRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Service called...");
        UsersQueues.CreateUserQueue(request.RoomName, request.UserName);
        return await Task.FromResult(new RoomRegistrationResponse { Joined = true });
    }

    public override async Task<NewStreamStatus> SendNewsFlash(
        IAsyncStreamReader<NewFlash> requestStream,
        ServerCallContext context
    )
    {
        while (await requestStream.MoveNext())
        {
            var newsFlash = requestStream.Current;
            MessagesQueue.AddNewsToQueue(newsFlash);
            _logger.LogInformation(
                $"Received news flash: {newsFlash.NewsItem} at {newsFlash.NewsTime}"
            );
        }

        return new NewStreamStatus { Success = true };
    }

    public override async Task StartMonitoring(
        Empty request,
        IServerStreamWriter<ReceivedMessage> streamWriter,
        ServerCallContext context
    )
    {
        while (true)
        {
            if (MessagesQueue.GetMessagesCount() > 0)
            {
                var msg = MessagesQueue.GetNextMessage();
                await streamWriter.WriteAsync(msg);
            }
            // await streamWriter.WriteAsync(
            //     new ReceivedMessage
            //     {
            //         MessageTime = Timestamp.FromDateTime(DateTime.UtcNow),
            //         Content = "This is a monitored message.",
            //         User = "System",
            //     }
            // );
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public override async Task StartChat(
        IAsyncStreamReader<ChatMessage> incomingStream,
        IServerStreamWriter<ChatMessage> outgoingStream,
        ServerCallContext context
    )
    {
        // Wait for the first message to get the user name
        while (!await incomingStream.MoveNext())
        {
            await Task.Delay(100);
        }

        string userName = incomingStream.Current.User;
        string room = incomingStream.Current.Room;
        Console.WriteLine($"User {userName} connected to room {incomingStream.Current.Room}");

        // TEST TEST TEST TEST - TO USE ONLY WHEN TESTING WITH BLOOMRPC
        // UsersQueues.CreateUserQueue(room, userName);
        // END TEST END TEST END TEST

        // Get messages from the user
        var reqTask = Task.Run(async () =>
        {
            while (await incomingStream.MoveNext())
            {
                Console.WriteLine($"Message received: {incomingStream.Current.Content}");
                UsersQueues.AddMessageToRoom(
                    ConvertToReceivedMessage(incomingStream.Current),
                    incomingStream.Current.Room
                );
            }
        });

        // Check for messages to send to the user
        var respTask = Task.Run(async () =>
        {
            while (true)
            {
                var userMsg = UsersQueues.GetMessageForUser(userName);
                if (userMsg != null)
                {
                    var userMessage = ConvertToChatMessage(userMsg, room);
                    await outgoingStream.WriteAsync(userMessage);
                }
                if (MessagesQueue.GetMessagesCount() > 0)
                {
                    var news = MessagesQueue.GetNextMessage();
                    var newsMessage = ConvertToChatMessage(news, room);
                    await outgoingStream.WriteAsync(newsMessage);
                }

                await Task.Delay(200);
            }
        });

        // Keep the method running
        while (true)
        {
            await Task.Delay(10000);
        }
    }

    private static ReceivedMessage ConvertToReceivedMessage(ChatMessage chatMsg)
    {
        var rcMsg = new ReceivedMessage
        {
            Content = chatMsg.Content,
            MessageTime = chatMsg.MessageTime,
            User = chatMsg.User,
        };

        return rcMsg;
    }

    private static ChatMessage ConvertToChatMessage(ReceivedMessage rcMsg, string room)
    {
        var chatMsg = new ChatMessage
        {
            Content = rcMsg.Content,
            MessageTime = rcMsg.MessageTime,
            User = rcMsg.User,
            Room = room,
        };

        return chatMsg;
    }
}

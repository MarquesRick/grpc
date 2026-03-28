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

    public override Task<RoomRegistrationResponse> RegisterToRoom(
        RoomRegistrationRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Service called...");
        var rnd = new Random();
        var roomNum = rnd.Next(1, 100);
        _logger.LogInformation($"Room no. {roomNum} and name {request.RoomName} registered.");
        var resp = new RoomRegistrationResponse { RoomId = roomNum, RoomName = request.RoomName };
        return Task.FromResult(resp);
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
}

using gRoom.gRPC.Messages;
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
            _logger.LogInformation(
                $"Received news flash: {newsFlash.NewsItem} at {newsFlash.NewsTime}"
            );
        }

        return new NewStreamStatus { Success = true };
    }
}

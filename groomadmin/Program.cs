using Google.Protobuf.WellKnownTypes;
using gRoom.gRPC.Messages;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5071");
var client = new Groom.GroomClient(channel);

Console.WriteLine("*** Admin Console started ***");
Console.WriteLine("Listening...");

using var call = client.StartMonitoring(new Empty());
var cts = new CancellationTokenSource();

while (await call.ResponseStream.MoveNext(cts.Token))
{
    var message = call.ResponseStream.Current;
    Console.WriteLine(
        $"New message: {message.Content}, user: {message.User}, at: {message.MessageTime}"
    );
}

using gRoom.gRPC.Messages;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5071");
var client = new Groom.GroomClient(channel);

Console.WriteLine("Enter room name:");
var roomName = Console.ReadLine();
var response = client.RegisterToRoom(new RoomRegistrationRequest { RoomName = roomName });
Console.WriteLine($"Registered to room: {response.RoomName}");

Console.ReadLine();

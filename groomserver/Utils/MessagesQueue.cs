using Google.Protobuf.WellKnownTypes;
using gRoom.gRPC.Messages;

namespace gRoom.gRPC.Utils;

public class MessagesQueue
{
    private static Queue<ReceivedMessage> _queue;

    static MessagesQueue()
    {
        _queue = new Queue<ReceivedMessage>();
    }

    public static void AddNewsToQueue(NewFlash news)
    {
        var msg = new ReceivedMessage
        {
            Content = news.NewsItem,
            User = "NewsBot",
            MessageTime = Timestamp.FromDateTime(DateTime.UtcNow),
        };
        _queue.Enqueue(msg);
    }

    public static ReceivedMessage GetNextMessage()
    {
        return _queue.Dequeue();
    }

    public static int GetMessagesCount()
    {
        return _queue.Count;
    }
}

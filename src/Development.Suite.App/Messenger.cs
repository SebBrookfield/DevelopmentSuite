using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Development.Suite.Ipc.Common;
using IMessenger = Development.Suite.App.Plugin.IMessenger;

namespace Development.Suite.App;

public class Messenger : IMessenger
{
    private readonly IIpcMessageSender _messageSender;
    private readonly Dictionary<Guid, IpcModel> _messagesById;
    private readonly Dictionary<Guid, SemaphoreSlim> _semaphoresById;

    public Messenger(IIpcMessageSender messageSender)
    {
        _messageSender = messageSender;
        _messagesById = new Dictionary<Guid, IpcModel>();
        _semaphoresById = new Dictionary<Guid, SemaphoreSlim>();
    }

    public async Task<TReply> Send<TReply, TMessage>(TMessage message) where TReply : IpcModel where TMessage : IpcModel
    {
        return await Send<TReply, TMessage>(message, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
    }

    public async Task<TReply> Send<TReply, TMessage>(TMessage message, CancellationToken cancellationToken) where TReply : IpcModel where TMessage : IpcModel
    {
        _semaphoresById[message.MessageId] = new SemaphoreSlim(1);
        await Task.WhenAll(_messageSender.SendMessage(message), _semaphoresById[message.MessageId].WaitAsync(cancellationToken));
        var reply = (TReply) _messagesById[message.MessageId];
        _messagesById.Remove(message.MessageId);
        return reply;
    }

    public async Task Send<TMessage>(TMessage message) where TMessage : IpcModel
    {
        await _messageSender.SendMessage(message);
    }

    public void ReceiveMessage(IpcModel? message)
    {
        if (message == null || !_semaphoresById.TryGetValue(message.MessageId, out var semaphoreSlim))
            return;

        _messagesById.Add(message.MessageId, message);
        semaphoreSlim.Release();
        semaphoreSlim.Dispose();
        _semaphoresById.Remove(message.MessageId);
    }
}
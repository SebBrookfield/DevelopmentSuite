using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Development.Suite.App.ExtensionMethods;
using Development.Suite.Ipc.Common;
using IMessenger = Development.Suite.App.Plugin.IMessenger;

namespace Development.Suite.App;

public class Messenger : IMessenger
{
    private readonly IIpcMessageSender _messageSender;
    private readonly Dictionary<Guid, IpcModel> _messagesById;
    private readonly Dictionary<Guid, ManualResetEventSlim> _resetEventById;

    public Messenger(IIpcMessageSender messageSender)
    {
        _messageSender = messageSender;
        _messagesById = new Dictionary<Guid, IpcModel>();
        _resetEventById = new Dictionary<Guid, ManualResetEventSlim>();
    }

    public async Task<TReply> Send<TReply, TMessage>(TMessage message) where TReply : IpcModel where TMessage : IpcModel
    {
        return await Send<TReply, TMessage>(message, TimeSpan.FromMinutes(1));
    }

    public async Task<TReply> Send<TReply, TMessage>(TMessage message, TimeSpan timeout) where TReply : IpcModel where TMessage : IpcModel
    {
        _resetEventById[message.MessageId] = new ManualResetEventSlim(false);
        await Task.WhenAll(_messageSender.SendMessage(message), _resetEventById[message.MessageId].WaitAsync(timeout));
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
        if (message == null || !_resetEventById.TryGetValue(message.MessageId, out var semaphoreSlim))
            return;

        _messagesById.Add(message.MessageId, message);
        semaphoreSlim.Set();
        semaphoreSlim.Dispose();
        _resetEventById.Remove(message.MessageId);
    }
}
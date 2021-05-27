using Shared.Messaging;
using System;
using System.Threading.Tasks;

namespace MailService.MessageHandlers
{
    internal abstract class MailingServiceHandlerBase<T>: IMessageHandler<T> where T : class
    {
        public async Task HandleMessageAsync(string messageType, T message)
        {
            Console.WriteLine($"Message with type {typeof(T)} received, starting handle...");
            Command(message);
        }

        protected abstract void Command(T message);
    }
}

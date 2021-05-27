using MailService.Messages;
using System;

namespace MailService.MessageHandlers
{
    class TestingHandler : MailingServiceHandlerBase<Test>
    {
        protected override void Command(Test message)
        {
            if(message != null)
            {
                Console.WriteLine($"Received message with id {message.Id} and name {message.Username} ");
            }
            else
            {
                Console.WriteLine($"Received message but value was null! ");
            }
        }
    }
}

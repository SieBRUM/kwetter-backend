using MailService.Messages;
using System;

namespace MailService.MessageHandlers
{
    class TestingHandler : MailingServiceHandlerBase<Test>
    {
        protected override void Command(Test message)
        {
            Console.WriteLine("====================== TestingHandler =============================");
        }
    }
}

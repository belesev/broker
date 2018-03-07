using BrokerAlgo.Helpers;
using NUnit.Framework;

namespace Tests
{
    class TestEmailSender
    {
        [Test]
        public void Test()
        {
            var sender = new EmailSender("smtp.gmail.com", 587, "belesev@gmail.com", "mtaopuyhkahbcjxr");
            sender.Send("Test subject", "Test body");
        }
    }
}

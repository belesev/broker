using System;
using QuikSharp;

namespace Broker01
{
    internal class Program
    {
        public static void Main()
        {
            var quik = new Quik(Quik.DefaultPort, new InMemoryStorage());    // инициализируем объект Quik
            var isServerConnected = quik.Service.IsConnected().Result;
            Console.WriteLine("This line is not going to be written");
        }
    }
}
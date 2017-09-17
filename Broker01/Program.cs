using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;

namespace Broker01
{
    class Program
    {
        static void Main(string[] args)
        {
            var quik = new Quik(Quik.DefaultPort, new InMemoryStorage());    // инициализируем объект Quik
            var isServerConnected = quik.Service.IsConnected().Result;
        }
    }
}
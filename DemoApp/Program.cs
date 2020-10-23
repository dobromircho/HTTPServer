using SIS.HTTP;
using System;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpServer = new HttpServer(80);
            await httpServer.StartAsync();
        }
    }
}

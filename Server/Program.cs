using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Example.Grpc.Chat;
using Grpc.Core;

namespace Server
{
    class Program
    {
        const string Host = "localhost";
        const int Port = 8080;

        public static void Main(string[] args)
        {
            var grpcServer = new Grpc.Core.Server
            {
                Services = { ChatService.BindService(new ChatServiceImpl()) },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };

            grpcServer.Start();

            Console.WriteLine("Port: " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            grpcServer.ShutdownAsync().Wait();
        }
    }
}

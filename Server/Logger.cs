using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Example.Grpc.Chat;
using Grpc.Core;
using Proto;

namespace Server
{
    class Logger
    {
        public static void LogUserAction(ConnectionStatus request, ServerCallContext context)
        {
            if (request.UserAction.ActionType == UserAction.Types.ActionType.Leave)
            {
                Console.WriteLine("user: " + request.UserAction.Name + " left at:" + DateTime.Now);
            }
            else
            {
                Console.WriteLine("user: " + request.UserAction.Name + " entered at:" + DateTime.Now);
            }
        }

        public static void LogChatMessage(IAsyncStreamReader<ChatMessage> requestStream)
        {
            Console.WriteLine("user: " + requestStream.Current.From + ", send message: " +
                              requestStream.Current.Message +
                              " at: " + DateTime.Now);
        }
    }

}

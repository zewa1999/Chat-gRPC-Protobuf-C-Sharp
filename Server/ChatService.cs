using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Example.Grpc.Chat;
using Grpc.Core;
using Proto;

namespace Server
{
    public class ChatServiceImpl : ChatService.ChatServiceBase
    {
        private static HashSet<IServerStreamWriter<ChatMessageFromServer>> responseStreams = new HashSet<IServerStreamWriter<ChatMessageFromServer>>();

        public override Task<Empty> onlineStatus(ConnectionStatus request, ServerCallContext context)
        {
            Logger.LogUserAction(request, context);
            return Task.FromResult(new Empty { });
        }

        public override async Task chat(IAsyncStreamReader<ChatMessage> requestStream,
            IServerStreamWriter<ChatMessageFromServer> responseStream,
            ServerCallContext context)
        {
            
            responseStreams.Add(responseStream);
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                Logger.LogChatMessage(requestStream);
                var messageFromClient = requestStream.Current;
                var message = new ChatMessageFromServer
                {
                    Message = messageFromClient
                };

                foreach (var stream in responseStreams)
                {
                    await stream.WriteAsync(message);
                }
            }
        }
    }
}

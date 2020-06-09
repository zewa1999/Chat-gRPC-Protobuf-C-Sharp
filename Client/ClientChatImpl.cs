using Grpc.Core;
using System;
using Com.Example.Grpc.Chat;
using System.Threading;
using Proto;

namespace Client
{
    public static class ClientChatImpl
    {
        private const string Host = "localhost";
        private const int Port = 8080;

        private static ChatService.ChatServiceClient _chatService;
        private static AsyncDuplexStreamingCall<ChatMessage, ChatMessageFromServer> _call;

        public static string ClientName;

        static ClientChatImpl()
        {
            // Create a channel
            var channel = new Channel(Host + ":" + Port, ChannelCredentials.Insecure);

            // Create a client with the channel
            _chatService = new ChatService.ChatServiceClient(channel);

            openConnectionServer();
        }

        public static async void openConnectionServer()
        {
            // Open a connection to the server
            try
            {
                using (_call = _chatService.chat())
                {
                    // Read messages from the response/service stream
                    while (await _call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var serverMessage = _call.ResponseStream.Current;
                        var otherClientMessage = serverMessage.Message;

                        MessageFormatter.RecieveMessage(otherClientMessage.From, otherClientMessage.Message);
                        ManelePlayer.RecieveMessage(otherClientMessage.Message);
                    }
                }
            }
            catch (RpcException)
            {
                _call = null;
                throw;
            }
        }

        public static async void sendButton_Execute(string clientMessage)
        {
            // Create a chat message
            var message = new ChatMessage
            {
                From = ClientName,
                Message = clientMessage
            };
            // Send the message

            if (_call != null)
            {
                await _call.RequestStream.WriteAsync(message);
            }
        }

        public static void ChatClosing()
        {
            _call.RequestStream.CompleteAsync();

            if (_call != null)
            {
                _chatService.onlineStatusAsync(new ConnectionStatus
                {
                    UserAction = new UserAction
                    {
                        ActionType = UserAction.Types.ActionType.Leave,
                        Name = ClientName
                    }
                });
            }
        }

        public static void UserEnteredChat()
        {
            _chatService.onlineStatusAsync(new ConnectionStatus
            {
                UserAction = new UserAction
                {
                    ActionType = UserAction.Types.ActionType.Join,
                    Name = ClientName
                }
            });
        }
    }
}

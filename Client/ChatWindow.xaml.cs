using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();

            MessageFormatter.SubscribeToMessageReciever(UpdateChatTextBlock);
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            string message = txt_message.Text;

            ClientChatImpl.sendButton_Execute(message);

            txt_message.Clear();
        }

        private void UpdateChatTextBlock(string name, List<Tuple<string, string>> list)
        {
            txt_chat.Inlines.Add(new Run($"{name}: "));

            foreach (Tuple<string, string> tuple in list)
            {
                var format = tuple.Item1;
                var text = tuple.Item2;

                FontWeight fontWeigth = FontWeights.Normal;
                FontStyle fontStyle = FontStyles.Normal;
                TextDecorationCollection textDecoration = new TextDecorationCollection();

                if (format.Contains('*'))
                    fontWeigth = FontWeights.Bold;

                if (format.Contains('_'))
                    fontStyle = FontStyles.Italic;

                if (format.Contains('~'))
                    textDecoration.Add(TextDecorations.Strikethrough);

                if (format.Contains('`'))
                    textDecoration.Add(TextDecorations.Underline);


                txt_chat.Inlines.Add(new Run(text)
                {
                    FontWeight = fontWeigth,
                    FontStyle = fontStyle,
                    TextDecorations = textDecoration
                });
            }


            txt_chat.Inlines.Add(new Run("\n"));

            scrollView.ScrollToBottom();
        }

        private void ChatWindow_OnClosed(object sender, EventArgs e)
        {
            ClientChatImpl.ChatClosing();
        }
    }
}

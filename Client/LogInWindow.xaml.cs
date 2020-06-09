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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void btn_join_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;

            if (!ValidateUsername(username))
            {
                MessageBox.Show("Type in a valid username.", "", MessageBoxButton.OK);
                return;
            }

            ClientChatImpl.ClientName = username;
            ClientChatImpl.UserEnteredChat();
            var window = new ChatWindow();
            window.Show(); // open the chat window

            Close(); // closes this window
        }

        private bool ValidateUsername(string username)
        {
            if (username == string.Empty)
                return false;

            return true;
        }
    }
}

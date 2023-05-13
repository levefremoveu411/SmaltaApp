using System.Windows;

namespace SmaltaApp.View.Windows
{
    public partial class Warning : Window
    {
        public Warning(string mess)
        {
            InitializeComponent();
            Message.Text = mess;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

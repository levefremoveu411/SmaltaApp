using SmaltaApp.ViewModel;
using System.Windows.Controls;


namespace SmaltaApp.View
{
    public partial class Analytics : Page
    {
        public Analytics()
        {
            InitializeComponent();
            DataContext = new AnalyticsVM();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}

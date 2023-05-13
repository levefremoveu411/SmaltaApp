using SmaltaApp.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;


namespace SmaltaApp.View
{
    /// <summary>
    /// Логика взаимодействия для Estimate.xaml
    /// </summary>
    public partial class Estimate : Page
    {
        public Estimate(int projectId)
        {
            InitializeComponent();
            DataContext = new EstimateVM(projectId);
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789,".IndexOf(e.Text) < 0;
        }

        //Дополнительная проверка
        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = (TextBox)e.Source;
            if (txt.Text != "")
            {
                try
                {
                    Convert.ToDouble(txt.Text);
                }
                catch
                {
                    txt.Focus();
                }
            }
        }
        //Дополнительная проверка
        private void TextBox_LostKeyboardFocus_2(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = (TextBox)e.Source;
            if (txt.Text != "")
            {
                try
                {
                    Convert.ToInt32(txt.Text);
                }
                catch
                {
                    txt.Focus();
                }
            }
        }
    }
}

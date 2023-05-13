using SmaltaApp.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmaltaApp.View
{
    public partial class BuildingСharacteristics : Page
    {
        public BuildingСharacteristics(int projectId)
        {
            InitializeComponent();
            DataContext = new BuildingСharacteristicsVM(projectId);
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

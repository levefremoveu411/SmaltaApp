using SmaltaApp.ViewModel;
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

namespace SmaltaApp.View
{
    public partial class EngineeringSystems : Page
    {
        public EngineeringSystems(int projectId)
        {
            InitializeComponent();
            DataContext = new EngineeringSystemsVM(projectId);
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
    }
}

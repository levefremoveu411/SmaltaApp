using SmaltaApp.Model;
using SmaltaApp.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SmaltaApp.View
{
    public partial class BasicData : Page
    {
        readonly Project Project;
        public static Image? myImageProject;
        public BasicData(Project project)
        {
            InitializeComponent();
            Project = project;
            myImageProject = ImageProject;
            DataContext = new BasicDataVM(Project);
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789.-/".IndexOf(e.Text) < 0;
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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

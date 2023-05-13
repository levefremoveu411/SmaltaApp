using SmaltaApp.Model.AdditionalClasses;
using SmaltaApp.Model.HelperClasses;
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
using System.Windows.Shapes;

namespace SmaltaApp.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSoftware.xaml
    /// </summary>
    public partial class AddSoftware : Window
    {
        public Software NewSoftware { get; private set; }
        public AddSoftware(Software s)
        {
            InitializeComponent();
            NewSoftware = s;
            DataContext = NewSoftware;
        }

        //Перемещение окна программы
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (NewSoftware.Name != null && NewSoftware.Developer != null && NewSoftware.Сertificate != null)
                DialogResult = true;
        }
    }
}

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
    /// Логика взаимодействия для AddNewValue.xaml
    /// </summary>
    public partial class AddEntry : Window
    {
        public Entry NewEntry { get; private set; }
        public AddEntry(Entry entry)
        {
            InitializeComponent();
            NewEntry = entry;
            DataContext = NewEntry;
        }

        //Перемещение окна программы
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (NewEntry.Name != null && NewEntry.Name?.Replace(" ", "").Length != 0)
                DialogResult = true;
        }
    }
}

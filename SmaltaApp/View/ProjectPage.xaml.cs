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
    public partial class ProjectPage : Page
    {
        public static Frame? MyProjectFrame;
        public ProjectPage(Model.Project project)
        {
            InitializeComponent();
            MyProjectFrame = ProjectFrame;
            DataContext = new ProjectVM(project);
        }
    }
}

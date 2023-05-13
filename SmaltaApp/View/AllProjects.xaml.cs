using SmaltaApp.ViewModel;
using System.Windows.Controls;


namespace SmaltaApp.View
{
    public partial class AllProjects : Page
    {
        public static Image? myMainImage;
        public AllProjects()
        {
            InitializeComponent();
            myMainImage = mainImage;
            DataContext = new AllProjectsVM();
        }
    }
}

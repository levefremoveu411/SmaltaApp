
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmaltaApp.Model
{
    public class ProjectFluctuation : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы AllProjects
        int? projectId;
        public int? ProjectId
        {
            get { return projectId; }
            set
            {
                projectId = value;
                OnPropertyChanged("ProjectId");
            }
        }

        //Ссылка на проект
        Project? project;
        public Project? Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        //Количество форм
        int? countForm;
        public int? CountForm
        {
            get { return countForm; }
            set
            {
                countForm = value;
                OnPropertyChanged("CountForm");
            }
        }

        //W
        double? w;
        public double? W
        {
            get { return w; }
            set
            {
                w = value;
                OnPropertyChanged("W");
            }
        }

        //F
        double? f;
        public double? F
        {
            get { return f; }
            set
            {
                f = value;
                OnPropertyChanged("F");
            }
        }

        //Т
        double? t;
        public double? T
        {
            get { return t; }
            set
            {
                t = value;
                OnPropertyChanged("T");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

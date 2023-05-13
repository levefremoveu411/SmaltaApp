using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmaltaApp.Model
{
    public class BuildingHeight : INotifyPropertyChanged
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

        //Количество наземных этажей
        int? countGroundFloors = 0;
        public int? CountGroundFloors
        {
            get { return countGroundFloors; }
            set
            {
                countGroundFloors = value;
                OnPropertyChanged("CountGroundFloors");
            }
        }

        //Количество цокольных этажей
        int? countBasements = 0;
        public int? CountBasements
        {
            get { return countBasements; }
            set
            {
                countBasements = value;
                OnPropertyChanged("CountBasements");
            }
        }

        //Высота первого этажа
        double? firstFloor = 0;
        public double? FirstFloor
        {
            get { return firstFloor; }
            set
            {
                firstFloor = value;
                OnPropertyChanged("FirstFloor");
            }
        }

        //Высота второго этажа
        double? secondFloor = 0;
        public double? SecondFloor
        {
            get { return secondFloor; }
            set
            {
                secondFloor = value;
                OnPropertyChanged("SecondFloor");
            }
        }

        //Высота типового этажа
        double? typicalFloor = 0;
        public double? TypicalFloor
        {
            get { return typicalFloor; }
            set
            {
                typicalFloor = value;
                OnPropertyChanged("TypicalFloor");
            }
        }

        //Высота цокольного этажа
        double? groundFloor = 0;
        public double? GroundFloor
        {
            get { return groundFloor; }
            set
            {
                groundFloor = value;
                OnPropertyChanged("GroundFloor");
            }
        }

        //Высота технического этажа
        double? technicalFloor = 0;
        public double? TechnicalFloor
        {
            get { return technicalFloor; }
            set
            {
                technicalFloor = value;
                OnPropertyChanged("TechnicalFloor");
            }
        }

        //Высота нулевого этажа
        double? nullFloor = 0;
        public double? NullFloor
        {
            get { return nullFloor; }
            set
            {
                nullFloor = value;
                OnPropertyChanged("NullFloor");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

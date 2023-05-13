using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model
{
    public class FireResistance : INotifyPropertyChanged
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

        //Сечение_600
        int? section600;
        public int? Section600
        {
            get { return section600; }
            set
            {
                section600 = value;
                OnPropertyChanged("Section600");
            }
        }

        //Сечение_500
        int? section500;
        public int? Section500
        {
            get { return section500; }
            set
            {
                section500 = value;
                OnPropertyChanged("Section500");
            }
        }

        //Сечение_400
        int? section400;
        public int? Section400
        {
            get { return section400; }
            set
            {
                section400 = value;
                OnPropertyChanged("Section400");
            }
        }

        //Балки
        int? beams;
        public int? Beams
        {
            get { return beams; }
            set
            {
                beams = value;
                OnPropertyChanged("Beams");
            }
        }

        //Стены
        int? walls;
        public int? Walls
        {
            get { return walls; }
            set
            {
                walls = value;
                OnPropertyChanged("Walls");
            }
        }

        //Перекрытия
        int? coatings;
        public int? Coatings
        {
            get { return coatings; }
            set
            {
                coatings = value;
                OnPropertyChanged("Coatings");
            }
        }

        //Марши
        int? flightsOfStairs;
        public int? FlightsOfStairs
        {
            get { return flightsOfStairs; }
            set
            {
                flightsOfStairs = value;
                OnPropertyChanged("FlightsOfStairs");
            }
        }

        //Перегородки
        int? partitions;
        public int? Partitions
        {
            get { return partitions; }
            set
            {
                partitions = value;
                OnPropertyChanged("Partitions");
            }
        }

        //Связи
        int? connections;
        public int? Connections
        {
            get { return connections; }
            set
            {
                connections = value;
                OnPropertyChanged("Connections");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

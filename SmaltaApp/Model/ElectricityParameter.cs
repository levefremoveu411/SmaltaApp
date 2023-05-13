using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmaltaApp.Model
{
    public class ElectricityParameter : INotifyPropertyChanged
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

        //Энергоснабжение магазинов
        double? shopsArea = 0;
        public double? ShopsArea
        {
            get { return shopsArea; }
            set
            {
                shopsArea = value;
                OnPropertyChanged("ShopsArea");
            }
        }

        //Энергоснабжение офисов
        double? officesArea = 0;
        public double? OfficesArea
        {
            get { return officesArea; }
            set
            {
                officesArea = value;
                OnPropertyChanged("OfficesArea");
            }
        }

        //Энергоснабжение квартир
        double? apartmentsArea = 0;
        public double? ApartmentsArea
        {
            get { return apartmentsArea; }
            set
            {
                apartmentsArea = value;
                OnPropertyChanged("ApartmentsArea");
            }
        }

        //Энергоснабжение прочих помещений
        double? otherArea = 0;
        public double? OtherArea
        {
            get { return otherArea; }
            set
            {
                otherArea = value;
                OnPropertyChanged("OtherArea");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class HeatParameter : INotifyPropertyChanged
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

        //Теплоснабжение магазинов
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

        //Теплоснабжение офисов
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

        //Теплоснабжение квартир
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

        //Теплоснабжение прочих помещений
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

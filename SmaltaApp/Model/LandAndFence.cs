using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class LandAndFence : INotifyPropertyChanged
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

        #region Показатели земельного участка
        //Площадь участка
        double? landArea;
        public double? LandArea
        {
            get { return landArea; }
            set
            {
                landArea = value;
                OnPropertyChanged("LandArea");
            }
        }

        //Площадь асфальта
        double? asphaltArea;
        public double? AsphaltArea
        {
            get { return asphaltArea; }
            set
            {
                asphaltArea = value;
                OnPropertyChanged("AsphaltArea");
            }
        }

        //Площадь озеленения
        double? greenArea;
        public double? GreenArea
        {
            get { return greenArea; }
            set
            {
                greenArea = value;
                OnPropertyChanged("GreenArea");
            }
        }
        #endregion

        #region Показатели эффективности огр. конструкций
        //Сопротивление теплопередачи внешних стен
        double? walls;
        public double? WallsST
        {
            get { return walls; }
            set
            {
                walls = value;
                OnPropertyChanged("WallsST");
            }
        }

        //Сопротивление теплопередачи покрытия
        double? coatings;
        public double? CoatingsST
        {
            get { return coatings; }
            set
            {
                coatings = value;
                OnPropertyChanged("CoatingsST");
            }
        }

        //Сопротивление теплопередачи окон
        double? windows;
        public double? WindowsST
        {
            get { return windows; }
            set
            {
                windows = value;
                OnPropertyChanged("WindowsST");
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using SmaltaApp.Model.AdditionalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class ClimaticCondition : INotifyPropertyChanged
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

        //Регион
        string? region;
        public string? Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        //Подрегион
        string? subRegion;
        public string? SubRegion
        {
            get { return subRegion; }
            set
            {
                subRegion = value;
                OnPropertyChanged("SubRegion");
            }
        }

        //Температура холодного дня
        int? temperatureColdD;
        public int? TemperatureColdD
        {
            get { return temperatureColdD; }
            set
            {
                temperatureColdD = value;
                OnPropertyChanged("TemperatureColdD");
            }
        }

        //Температура холодной недели
        int? temperatureColdW;
        public int? TemperatureColdW
        {
            get { return temperatureColdW; }
            set
            {
                temperatureColdW = value;
                OnPropertyChanged("TemperatureColdW");
            }
        }

        //Внешний ключ таблицы SnowyRegions
        int? snowyRegionId;
        public int? SnowyRegionId
        {
            get { return snowyRegionId; }
            set
            {
                snowyRegionId = value;
                OnPropertyChanged("SnowyRegionId");
            }
        }

        //Ссылка на снеговой регион
        SnowyRegion? snowyRegion;
        public SnowyRegion? SnowyRegion
        {
            get { return snowyRegion; }
            set
            {
                snowyRegion = value;
                OnPropertyChanged("SnowyRegion");
            }
        }

        //Внешний ключ таблицы WindRegions
        int? windRegionId;
        public int? WindRegionId
        {
            get { return windRegionId; }
            set
            {
                windRegionId = value;
                OnPropertyChanged("WindRegionId");
            }
        }

        //Ссылка на ветровой регион
        WindRegion? windRegion;
        public WindRegion? WindRegion
        {
            get { return windRegion; }
            set
            {
                windRegion = value;
                OnPropertyChanged("WindRegion");
            }
        }

        //Внешний ключ таблицы Responsibilities
        int? responsibilityId;
        public int? ResponsibilityId
        {
            get { return responsibilityId; }
            set
            {
                responsibilityId = value;
                OnPropertyChanged("ResponsibilityId");
            }
        }

        //Ссылка на ответсвенность
        Responsibility? responsibility;
        public Responsibility? Responsibility
        {
            get { return responsibility; }
            set
            {
                responsibility = value;
                OnPropertyChanged("Responsibility");
            }
        }

        //Карта
        string? map;
        public string? Map
        {
            get { return map; }
            set
            {
                map = value;
                OnPropertyChanged("Map");
            }
        }

        //Сейсмичность (тест)
        int? seismicityTest;
        public int? SeismicityTest
        {
            get { return seismicityTest; }
            set
            {
                seismicityTest = value;
                OnPropertyChanged("SeismicityTest");
            }
        }

        //Сейсмичность по проекту
        int? seismicityProject;
        public int? SeismicityProject
        {
            get { return seismicityProject; }
            set
            {
                seismicityProject = value;
                OnPropertyChanged("SeismicityProject");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

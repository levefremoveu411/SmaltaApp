using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class BuildingVolume : INotifyPropertyChanged
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

        //Наземная часть
        double? groundPart = 0;
        public double? GroundPart
        {
            get { return groundPart; }
            set
            {
                groundPart = value;
                OnPropertyChanged("GroundPart");
            }
        }

        //Цокольная часть
        double? basement = 0;
        public double? Basement
        {
            get { return basement; }
            set
            {
                basement = value;
                OnPropertyChanged("Basement");
            }
        }

        //Входная часть
        double? entrancePart = 0;
        public double? EntrancePart
        {
            get { return entrancePart; }
            set
            {
                entrancePart = value;
                OnPropertyChanged("EntrancePart");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

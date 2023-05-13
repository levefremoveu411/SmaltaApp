using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class Charecteristic : INotifyPropertyChanged
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

        //Категория долговечности
        string? category;
        public string? Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        //Срок эксплуатации
        string? lifeTime;
        public string? LifeTime
        {
            get { return lifeTime; }
            set
            {
                lifeTime = value;
                OnPropertyChanged("LifeTime");
            }
        }

        //Огнестойкость
        int? fireResistance;
        public int? FireResistance
        {
            get { return fireResistance; }
            set
            {
                fireResistance = value;
                OnPropertyChanged("FireResistance");
            }
        }

        //Площадь застройки
        double? builtUpArea;
        public double? BuiltUpArea
        {
            get { return builtUpArea; }
            set
            {
                builtUpArea = value;
                OnPropertyChanged("BuiltUpArea");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

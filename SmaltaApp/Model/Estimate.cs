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
    public class Estimate : INotifyPropertyChanged
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

        //Внешний ключ таблицы Chapters
        int? chapterId;
        public int? ChapterId
        {
            get { return chapterId; }
            set
            {
                chapterId = value;
                OnPropertyChanged("ChapterId");
            }
        }

        //Ссылка на критерий оплаты
        Chapter? chapter;
        public Chapter? Chapter
        {
            get { return chapter; }
            set
            {
                chapter = value;
                OnPropertyChanged("Chapter");
            }
        }

        //Выплата
        double? pay;
        public double? Pay
        {
            get { return pay; }
            set
            {
                pay = value;
                OnPropertyChanged("Pay");
            }
        }

        //Трудозатраты
        int? laborCosts;
        public int? LaborCosts
        {
            get { return laborCosts; }
            set
            {
                laborCosts = value;
                OnPropertyChanged("LaborCosts");
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model.AdditionalClasses
{
    public class RationaleInfo : INotifyPropertyChanged
    {
        //Код проекта
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

        //Картинка
        byte[]? imageOne;
        public byte[]? ImageOne
        {
            get { return imageOne; }
            set
            {
                imageOne = value;
                OnPropertyChanged("ImageOne");
            }
        }

        //Картинка
        byte[]? imageTwo;
        public byte[]? ImageTwo
        {
            get { return imageTwo; }
            set
            {
                imageTwo = value;
                OnPropertyChanged("ImageTwo");
            }
        }

        //Внешний ключ таблицы Software
        int? softwareId;
        public int? SoftwareId
        {
            get { return softwareId; }
            set
            {
                softwareId = value;
                OnPropertyChanged("SoftwareId");
            }
        }

        //Ссылка 
        Software? software;
        public Software? Software
        {
            get { return software; }
            set
            {
                software = value;
                OnPropertyChanged("Software");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

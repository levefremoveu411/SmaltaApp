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
    public class FunctionalPurpose :INotifyPropertyChanged
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


        //Внешний ключ таблицы Appointments
        int? appointmentId;
        public int? AppointmentId
        {
            get { return appointmentId; }
            set
            {
                appointmentId = value;
                OnPropertyChanged("AppointmentId");
            }
        }

        //Ссылка на функцию этажа
        Appointment? appointment;
        public Appointment? Appointment
        {
            get { return appointment; }
            set
            {
                appointment = value;
                OnPropertyChanged("Appointment");
            }
        }


        //Внешний ключ таблицы FloorTypes
        int? floorTypetId;
        public int? FloorTypeId
        {
            get { return floorTypetId; }
            set
            {
                floorTypetId = value;
                OnPropertyChanged("FloorTypeId");
            }
        }

        //Ссылка на тип этажа
        FloorType? floorType;
        public FloorType? FloorType
        {
            get { return floorType; }
            set
            {
                floorType = value;
                OnPropertyChanged("FloorType");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

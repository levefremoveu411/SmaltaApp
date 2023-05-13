using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model
{
    public class BuildingArea : INotifyPropertyChanged
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

        #region Общая площадь помещения
        //Площадь магазинов
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

        //Площадь офисов
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

        //Площадь квартир
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

        //Площадь прочих помещений
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
        #endregion

        #region Полезная площадь помещения
        //Полезная площадь магазинов
        double? effectiveShopsArea = 0;
        public double? EffectiveShopsArea
        {
            get { return effectiveShopsArea; }
            set
            {
                effectiveShopsArea = value;
                OnPropertyChanged("EffectiveShopsArea");
            }
        }

        //Полезная площадь офисов
        double? effectiveOfficesArea = 0;
        public double? EffectiveOfficesArea
        {
            get { return effectiveOfficesArea; }
            set
            {
                effectiveOfficesArea = value;
                OnPropertyChanged("EffectiveOfficesArea");
            }
        }

        //Полезная площадь квартир
        double? effectiveApartmentsArea = 0;
        public double? EffectiveApartmentsArea
        {
            get { return effectiveApartmentsArea; }
            set
            {
                effectiveApartmentsArea = value;
                OnPropertyChanged("EffectiveApartmentsArea");
            }
        }

        //Полезная площадь прочих помещений
        double? effectiveOtherArea = 0;
        public double? EffectiveOtherArea
        {
            get { return effectiveOtherArea; }
            set
            {
                effectiveOtherArea = value;
                OnPropertyChanged("EffectiveOtherArea");
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

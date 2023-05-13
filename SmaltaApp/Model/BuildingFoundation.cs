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
    public class BuildingFoundation : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы AllProjects
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        //Внешний ключ таблицы FoundationTypes - тип фкндамента
        public int? FoundationTypeId { get; set; }
        public FoundationType? FoundationType { get; set; }

        //Внешний ключ таблицы TechnologyTypes - тип технологии смр
        public int? TechnologyTypeId { get; set; }
        public TechnologyType? TechnologyType { get; set; }

        //Тип фундаментной плиты 
        public int? Slab { get; set; }
        //Толщина плиты
        public double? ThicknessSlab { get; set; }

        //Тип фундаментной плиты с надбалкой
        public int? SlabTwo { get; set; }
        //Толщина плиты с надбалкой
        public double? ThicknessSlabTwo { get; set; }

        //Тип подколонников
        public int? UnderСolumns { get; set; }
        //Размерность подколонников
        public string? DimensionUnderСolumns { get; set; }

        //Тип колонн
        public int? Сolumns { get; set; }
        //Размерность колонн
        public string? DimensionСolumns { get; set; }

        //Стены
        public int? Walls { get; set; }
        //Толщина стен
        public double? ThicknessWalls { get; set; }

        //Перекрытия
        public int? Floors { get; set; }
        //Толщина перекрытия
        public double? ThicknessFloors { get; set; }

        //Ригели
        public int? Сrossbars { get; set; }
        //Толщина ригели
        public double? ThicknessСrossbars { get; set; }

        //Бетон класса B
        public int? ConcreteB { get; set; }
        //Бетон класса F
        public int? ConcreteF { get; set; }
        //Бетон класса W
        public int? ConcreteW { get; set; }
        //Марка стали
        public string? SteelGrade { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

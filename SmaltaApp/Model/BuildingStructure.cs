using SmaltaApp.Model.AdditionalClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SmaltaApp.Model
{
    public class BuildingStructure : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы AllProjects
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        //Несущие конструкции
        public int? BearingStructuresId { get; set; }
        //Колонны
        public int? ColumnsId { get; set; }
        //Перекрытия
        public int? FloorsId { get; set; }
        //Ригели
        public int? СrossbarsId { get; set; }
        //Стены
        public int? WallsId { get; set; }

        //Бетон класса B
        public int? ConcreteB { get; set; }
        //Бетон класса F
        public int? ConcreteF { get; set; }
        //Бетон класса W
        public int? ConcreteW { get; set; }
        //Марка стали
        public string? SteelGrade { get; set; }

        //Конструкция лестницы
        [ForeignKey("StairType")]
        public int? StairId { get; set; }
        public StairType? StairType { get; set; }
        //Косоур
        public string? Beam { get; set; }

        //Конструкция шахты лифта
        [ForeignKey("LiftType")]
        public int? LiftId { get; set; }
        public LiftType? LiftType { get; set; }
        //Толщина лифта
        public double? ThicknessLift { get; set; }

        //Конструкция крыши
        [ForeignKey("HouseTopType")]
        public int? HouseTopId { get; set; }
        public HouseTopType? HouseTopType { get; set; }
        //Водосток
        public string? Gully { get; set; }

        //Конструкция кровли
        [ForeignKey("RoofType")]
        public int? RoofTypeId { get; set; }
        public RoofType? RoofType { get; set; }

        //Материал кровли
        [ForeignKey("RoofingMaterial")]
        public int? RoofingMaterialId { get; set; }
        public RoofingMaterial? RoofingMaterial { get; set; }

        //Высота парапета
        public double? ParapetHeight { get; set; }
        //Примечание
        public string? ParapetNote { get; set; }

        //Бетон класса B
        public int? ConcreteBb { get; set; }
        //Бетон класса F
        public int? ConcreteFf { get; set; }
        //Бетон класса W
        public int? ConcreteWw { get; set; }
        //Толщина бетона
        public double? ThicknessConcrete { get; set; }

        //Подготовка
        public string? Preparation { get; set; }
        //Толщина подготовки
        public double? ThicknessPreparation { get; set; }

        //Вентшахта
        [ForeignKey("AirShaftType")]
        public int? AirShaftTypeId { get; set; }
        public AirShaftType? AirShaftType { get; set; }

        //Наружная отделка фасада
        [ForeignKey("ExteriorFinish")]
        public int? ExteriorFinishId { get; set; }
        public ExteriorFinish? ExteriorFinish { get; set; }

        //Материал утеплителя
        [ForeignKey("InsulationMaterial")]
        public int? InsulationMaterialId { get; set; }
        public InsulationMaterial? InsulationMaterial { get; set; }
        //Толщина утеплителя
        public double? ThicknessInsulation { get; set; }
        //Плотность утеплителя
        public string? Density { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

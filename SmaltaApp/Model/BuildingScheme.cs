using SmaltaApp.Model.AdditionalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class BuildingScheme : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы AllProjects
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        //Форма здания
        public string? Form { get; set; }

        //Сложность формы
        public string? Complexity { get; set; }

        //Длина здания
        public double? Length { get; set; }

        //Ширина здания
        public double? Width { get; set; }

        //Контруктивный тип здания
        [ForeignKey("ConstructiveType")]
        public int? ConstructiveTypeId { get; set; }
        public ConstructiveType? ConstructiveType { get; set; }

        //Поперечная схема
        public int? LateralSchemeId { get; set; }
        //Кол-во пролетов
        public int? CountLateralSpan { get; set; }
        //Шаги
        public string? LateralSteps { get; set; }

        //Продольная схема
        public int? LongitudinalSchemeId { get; set; }
        //Кол-во пролетов
        public int? CountLongitudinalSpan { get; set; }
        //Шаги
        public string? LongitudinalSteps { get; set; }

        //Вертикальная устойчивость
        public int? VerticalStabilityId { get; set; }
        //Горизонтальная устойчивость
        public int? HorizontalStabilityId { get; set; }
        //Поперечная устойчивость
        public int? LateralStabilityId { get; set; }
        //Продольная устойчивость
        public int? LongitudinalStabilityId { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

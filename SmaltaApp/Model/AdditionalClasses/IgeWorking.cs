using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model.AdditionalClasses
{
    public class IgeWorking 
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
            }
        }

        //Отчет
        public string? Report { get; set; }
        //Год выполнения
        public int? Year { get; set; }
        //Организация
        public string? Organization { get; set; }

    }
}

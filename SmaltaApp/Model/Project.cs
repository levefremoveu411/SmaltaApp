using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class Project : INotifyPropertyChanged
    {
        //Код проекта
        public int Id { get; set; }

        //Картинка проекта
        byte[]? image;
        public byte[]? Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        //Наменование проекта
        string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        //Год создания проекта
        int? year;
        public int? Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        //Внешний ключ таблицы Cities
        int? cityId;
        public int? CityId
        {
            get { return cityId; }
            set
            {
                cityId = value;
                OnPropertyChanged("CityId");
            }
        }

        //Ссылка на город
        City? city;
        public City? City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        //Внешний ключ таблицы Developers
        int? developerId;
        public int? DeveloperId
        {
            get { return developerId; }
            set
            {
                developerId = value;
                OnPropertyChanged("DeveloperId");
            }
        }

        //Ссылка на исполняющего
        Developer? developer;
        public Developer? Developer
        {
            get { return developer; }
            set
            {
                developer = value;
                OnPropertyChanged("Developer");
            }
        }

        //Роль
        string? role;
        public string? Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        //Внешний ключ таблицы Customers
        int? customerId;
        public int? CustomerId
        {
            get { return customerId; }
            set
            {
                customerId = value;
                OnPropertyChanged("CustomerId");
            }
        }

        //Ссылка на заказчика
        Customer? customer;
        public Customer? Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        //Договор
        string? treaty;
        public string? Treaty
        {
            get { return treaty; }
            set
            {
                treaty = value;
                OnPropertyChanged("Treaty");
            }
        }

        //Дата заключения договора
        DateTime? dateOfConclusion;
        public DateTime? DateOfConclusion
        {
            get { return dateOfConclusion; }
            set
            {
                dateOfConclusion = value;
                OnPropertyChanged("DateOfConclusion");
            }
        }

        //Документация
        string? documentation;
        public string? Documentation
        {
            get { return documentation; }
            set
            {
                documentation = value;
                OnPropertyChanged("Documentation");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

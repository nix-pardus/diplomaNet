using System;
using System.Collections.Generic;

//namespace ServiceCenterApi.Models
namespace diplomaNet
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName
        {
            get { return ToString(); }
        }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public override string ToString()
        {
            return LastName + " " + FirstName + " " + Patronymic;
        }
    }

}


//namespace ServiceCenterApi.Models
//{
//    public class Employee
//    {
//        public int Id { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public string? Patronymic { get; set; }
//        public DateTime Birthdate { get; set; }
//        public string Address { get; set; }
//        public string Phone { get; set; }
//        public List<Order> Orders { get; set; } = new List<Order>();
//    }
//}

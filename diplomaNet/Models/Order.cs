using System;
using System.Collections.Generic;

//namespace ServiceCenterApi.Models
namespace diplomaNet
{
    public enum Status
    {
        Added,
        Planned,
        AtWork,
        Completed
    }
    public class Order
    {
        public Order()
        {
            Status = Status.Added;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Client Client { get; set; }
        public List<Work> WorkList { get; set; }
        public Status Status { get; set; }
        public List<Employee> Employees { get; set; }
    }
}


//namespace ServiceCenterApi.Models
//{
//    public enum Status
//    {
//        Added,
//        Planned,
//        AtWork,
//        Completed
//    }
//    public class Order
//    {
//        public Order()
//        {
//            Status = Status.Added;
//        }

//        public int Id { get; set; }
//        public DateTime Date { get; set; }
//        public Client Client { get; set; }
//        public List<Work> WorkList { get; set; } = new List<Work>();
//        public Status Status { get; set; }
//        public List<Employee> Employees { get; set; } = new List<Employee>();
//    }
//}

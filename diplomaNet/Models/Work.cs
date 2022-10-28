using System.Collections.Generic;

//namespace ServiceCenterApi.Models
namespace diplomaNet
{
    public class Work
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}

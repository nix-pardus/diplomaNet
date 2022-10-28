//namespace ServiceCenterApi.Models
namespace diplomaNet
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName
        {
            get { return ToString(); }
        }
        public string Address { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return LastName + " " + FirstName + " " + Patronymic;
        }
        
    }
}

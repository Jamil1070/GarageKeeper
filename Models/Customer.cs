using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models






{
    public class Customer
    {
        
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public List<Car>? Cars { get; set; }

       

        public Customer()
        {
       
            

        }


        public Customer(string name, string email, string adress)
        {
            CustomerId = GenerateUniqueId();
            Name = name;
            Email = email;
            Adress = adress;
            Cars = new List<Car>();

        }


        private static int nextCustomerId = 1;

        private int GenerateUniqueId()
        {
            return nextCustomerId++;
        }
    }
}

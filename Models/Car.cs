using System.ComponentModel.DataAnnotations.Schema;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models




{
    public class Car
    {

        public int CarID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public string ChassisNumber { get; set; }

        public List<Order>? Orders { get; set; }

        public Customer? Customer { get; set; }

        public Car () {
           
            
        }


        public Car(string make, string model, string licensePlate, string chassisnumber, Customer customer)
        {
            CarID = GenerateUniqueId();
            Make = make;
            Model = model;
            LicensePlate = licensePlate;
            ChassisNumber = chassisnumber;
            Customer = customer;
            Orders = new List<Order>();
        }

        /*public void AssignCustomer(Customer customer)
        {
            Customer = customer;
        }
        */
        private static int nextCarId = 1;

        private int GenerateUniqueId()
        {
            return nextCarId++;
        }
    }
}

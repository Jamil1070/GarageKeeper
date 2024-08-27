using System.ComponentModel.DataAnnotations.Schema;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models





{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderDetails { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }


        public Car? Car { get; set; }
       
        public Order()
        {
            
        }

        public Order(Car car, string orderdetails)
        {
            Car = car;
            OrderId = GenerateUniqueId();

            OrderDetails = orderdetails;
        }

        private static int nextOrderId = 1;

        private int GenerateUniqueId()
        {
            return nextOrderId++;
        }
    }
}

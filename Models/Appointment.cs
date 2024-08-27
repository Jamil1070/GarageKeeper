using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models



{
    public class Appointment
    {

        public int AppointmentId { get; set; }


        [ForeignKey("Car")]
        public int CarID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; }
        public string RequiredService { get; set; }
        public string Status { get; set; }

        public Car? Car { get; set; }

        public Appointment()
        {
      
            
        }

        public Appointment(DateTime appointmentDate, Car car, string requiredService, string status)
        {
            AppointmentId = GenerateUniqueId();
            AppointmentDate = appointmentDate;
            Car = car;
            RequiredService = requiredService;
            Status = status;
        }


        private static int nextAppointmentId = 1;

        private int GenerateUniqueId()
        {
            return nextAppointmentId++;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; } // Primary Key

        [Required]
        [DataType(DataType.Date)]
        public DateTime BillDate { get; set; } // Date of the bill

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Total amount for the bill

        [Required]
        [StringLength(100)]
        public string Description { get; set; } // Description of the bill

        // Foreign Key for related entities
        public int? AppointmentId { get; set; } // Nullable if the bill is not related to an appointment

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } // Navigation property for related Appointment

        public int? CustomerId { get; set; } // Nullable if the bill is not related to a customer

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } // Navigation property for related Customer
    }
}

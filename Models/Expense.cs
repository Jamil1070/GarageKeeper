using System.ComponentModel.DataAnnotations;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models





{
    public class Expense
    {

        
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        public Expense()
        {
            
            
        }


        public Expense(string description, decimal amount, DateTime date)
        {
            Description = description;
            Amount = amount;
            Date = date;
            ExpenseId = GenerateUniqueId();
        }

        private static int nextTransactionId = 1;

        private int GenerateUniqueId()
        {
            return nextTransactionId++;
        }

        
    }
}

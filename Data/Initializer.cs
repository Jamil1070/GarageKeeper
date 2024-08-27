using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Data
{
    public class Initializer
    {
        

        public static void DbSetInitializer(GarageContext context)
        {
            Console.WriteLine("iniciando");

            context.Database.EnsureCreated();

			


            // Seeders

            if (!context.Customer.Any())
            {
                context.Add(new Customer { Name = "Lara", Email = "lara@example.com", Adress = "River Street 12", PhoneNumber = "0612345678" });
                context.Add(new Customer { Name = "Tom", Email = "tom@example.com", Adress = "Forest Road 9", PhoneNumber = "0687654321" });
                context.Add(new Customer { Name = "Mila", Email = "mila@example.com", Adress = "Sun Lane 5", PhoneNumber = "0611223344" });
                context.Add(new Customer { Name = "Lucas", Email = "lucas@example.com", Adress = "Harbor Street 16", PhoneNumber = "0677889900" });
                context.Add(new Customer { Name = "Emma", Email = "emma@example.com", Adress = "Village Square 3", PhoneNumber = "0622334455" });
                context.Add(new Customer { Name = "Noah", Email = "noah@example.com", Adress = "Park Lane 21", PhoneNumber = "0699887766" });
                context.Add(new Customer { Name = "Lily", Email = "lily@example.com", Adress = "Church Street 18", PhoneNumber = "0655443322" });
                context.Add(new Customer { Name = "Ethan", Email = "ethan@example.com", Adress = "Mill Road 7", PhoneNumber = "0633221100" });
                context.Add(new Customer { Name = "Sophia", Email = "sophia@example.com", Adress = "Hawthorn Lane 24", PhoneNumber = "0666554433" });
                context.Add(new Customer { Name = "James", Email = "james@example.com", Adress = "Main Street 30", PhoneNumber = "0644332211" });

                context.SaveChanges();
            }



            if (!context.Car.Any())
            {
                context.Add(new Car { Make = "Tesla", Model = "Model S", LicensePlate = "3XYZ789", ChassisNumber = "J1HGF234VBN87TR5E", CustomerId = 1 });
                context.Add(new Car { Make = "Jaguar", Model = "XF", LicensePlate = "3JKL123", ChassisNumber = "K9LMN345XCV89TY6U", CustomerId = 1 });
                context.Add(new Car { Make = "Land Rover", Model = "Range Rover", LicensePlate = "3ABC456", ChassisNumber = "L0QWE456VBN65XY7Z", CustomerId = 2 });
                context.Add(new Car { Make = "Porsche", Model = "Cayenne", LicensePlate = "3DEF789", ChassisNumber = "M8NOP567XCV21QW4R", CustomerId = 2 });
                context.Add(new Car { Make = "Lexus", Model = "RX 350", LicensePlate = "3GHI012", ChassisNumber = "N4RTS890VBN32LM8T", CustomerId = 3 });
                context.Add(new Car { Make = "Cadillac", Model = "Escalade", LicensePlate = "3JKL345", ChassisNumber = "O7XYZ234RTY67NB5V", CustomerId = 3 });
                context.Add(new Car { Make = "Infiniti", Model = "Q50", LicensePlate = "3MNO678", ChassisNumber = "P3QWE567XCV42OP9M", CustomerId = 4 });
                context.Add(new Car { Make = "Acura", Model = "MDX", LicensePlate = "3PQR901", ChassisNumber = "Q9RTY234VBN76MN4J", CustomerId = 4 });
                context.Add(new Car { Make = "Buick", Model = "Enclave", LicensePlate = "3STU234", ChassisNumber = "R5XYZ789RTY90JK6L", CustomerId = 5 });
                context.Add(new Car { Make = "GMC", Model = "Yukon", LicensePlate = "3VWX567", ChassisNumber = "S8QWE234XCV56BN7Y", CustomerId = 5 });
                context.Add(new Car { Make = "Alfa Romeo", Model = "Stelvio", LicensePlate = "3YZA890", ChassisNumber = "T2OPQ345RTY90MN1L", CustomerId = 6 });
                

                context.SaveChanges();
            }





           
                if (!context.Order.Any())
                {
                    context.Add(new Order { CarID = 1, OrderDetails = "Replace battery" });
                    context.Add(new Order { CarID = 1, OrderDetails = "Check exhaust system" });
                    context.Add(new Order { CarID = 2, OrderDetails = "Replace windshield wipers" });
                    context.Add(new Order { CarID = 2, OrderDetails = "Replace cabin air filter" });
                    context.Add(new Order { CarID = 3, OrderDetails = "Flush cooling system" });
                    context.Add(new Order { CarID = 3, OrderDetails = "Check power steering fluid" });
                    context.Add(new Order { CarID = 4, OrderDetails = "Replace fuel filter" });
                    context.Add(new Order { CarID = 4, OrderDetails = "Inspect exhaust manifold" });
                    context.Add(new Order { CarID = 5, OrderDetails = "Check and replace coolant" });
                    context.Add(new Order { CarID = 5, OrderDetails = "Replace brake fluid" });
                    context.Add(new Order { CarID = 6, OrderDetails = "Lubricate chassis components" });
                    context.Add(new Order { CarID = 6, OrderDetails = "Inspect and replace spark plug wires" });
                    context.Add(new Order { CarID = 7, OrderDetails = "Test battery and alternator" });
                    context.Add(new Order { CarID = 7, OrderDetails = "Replace serpentine belt" });
                    context.Add(new Order { CarID = 8, OrderDetails = "Inspect and clean throttle body" });


                    context.SaveChanges();
                }


            

            if (!context.Appointment.Any())
            {
                if (!context.Appointment.Any())
                {
                    context.Add(new Appointment { CarID = 1, AppointmentDate = DateTime.Now.AddDays(1), Status = "Confirmed", RequiredService = "Engine tune-up" });
                    context.Add(new Appointment { CarID = 2, AppointmentDate = DateTime.Now.AddDays(2), Status = "Pending", RequiredService = "Tire replacement" });
                    context.Add(new Appointment { CarID = 3, AppointmentDate = DateTime.Now.AddDays(3), Status = "Confirmed", RequiredService = "Suspension check" });
                    context.Add(new Appointment { CarID = 4, AppointmentDate = DateTime.Now.AddDays(4), Status = "Pending", RequiredService = "Battery testing" });
                    context.Add(new Appointment { CarID = 5, AppointmentDate = DateTime.Now.AddDays(5), Status = "Confirmed", RequiredService = "Headlight replacement" });
                    context.Add(new Appointment { CarID = 6, AppointmentDate = DateTime.Now.AddDays(6), Status = "Pending", RequiredService = "Brake fluid replacement" });
                    context.Add(new Appointment { CarID = 7, AppointmentDate = DateTime.Now.AddDays(7), Status = "Confirmed", RequiredService = "Steering alignment" });
                    context.Add(new Appointment { CarID = 8, AppointmentDate = DateTime.Now.AddDays(8), Status = "Pending", RequiredService = "Transmission diagnostics" });
                    context.Add(new Appointment { CarID = 9, AppointmentDate = DateTime.Now.AddDays(9), Status = "Confirmed", RequiredService = "AC system recharge" });
                    

                    context.SaveChanges();
                }


                
            }

            if (!context.Invoice.Any())
            {
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 115.75m, Details = "Engine repair", CarID = 1 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 85.00m, Details = "Transmission service", CarID = 2 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 130.40m, Details = "Full service and tune-up", CarID = 3 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 65.25m, Details = "Brake replacement", CarID = 4 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 95.10m, Details = "Radiator repair", CarID = 5 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 210.00m, Details = "Engine rebuild", CarID = 6 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 75.90m, Details = "Suspension check and repair", CarID = 7 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 55.30m, Details = "AC system service", CarID = 8 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 140.25m, Details = "Fuel system service", CarID = 9 });
                context.Add(new Invoice { IssueDate = DateTime.Now, TotalAmount = 170.00m, Details = "Complete transmission overhaul", CarID = 10 });
               

                context.SaveChanges();
            }

            if (!context.Expense.Any())
            {
                context.Add(new Expense { Description = "Office rent", Amount = 1200.00m, Date = DateTime.Now.AddDays(-5) });
                context.Add(new Expense { Description = "Client dinners", Amount = 180.75m, Date = DateTime.Now.AddDays(-10) });
                context.Add(new Expense { Description = "Printer cartridges", Amount = 45.50m, Date = DateTime.Now.AddDays(-15) });
                context.Add(new Expense { Description = "Team building event", Amount = 250.00m, Date = DateTime.Now.AddDays(-20) });
                context.Add(new Expense { Description = "Subscription fees", Amount = 85.00m, Date = DateTime.Now.AddDays(-25) });
                context.Add(new Expense { Description = "Marketing campaign", Amount = 320.00m, Date = DateTime.Now.AddDays(-30) });
                context.Add(new Expense { Description = "Phone bills", Amount = 55.40m, Date = DateTime.Now.AddDays(-35) });
                context.Add(new Expense { Description = "Domain renewal", Amount = 30.60m, Date = DateTime.Now.AddDays(-40) });
                

                context.SaveChanges();
            }

            

        }
    }
    
}

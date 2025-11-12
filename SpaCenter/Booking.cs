namespace SpaCenter
{

    public class Booking
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string PaymentMethod { get; set; }

        public string Status { get; private set; } 

        
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public Service Service { get; set; }
        public List<Service> Services { get; set; }

        public Booking(Customer customer, Employee employee, DateTime date, TimeSpan time, string paymentMethod)
        {
            Customer = customer;
            Employee = employee;
            Date = date;
            Time = time;
            PaymentMethod = paymentMethod;
            Status = "accepted";
            Services = new List<Service>();
        }

        public void UpdateBooking(DateTime newDate, TimeSpan newTime)
        {
            Date = newDate;
            Time = newTime;
            Console.WriteLine("Booking updated successfully.");
        }

        public void CancelBooking()
        {
            if (Status == "canceled")
            {
                Console.WriteLine("Booking is already canceled.");
                return;
            }

            Status = "canceled";
            Console.WriteLine("Booking canceled. Refund processed if payment was made online.");
        }

        public void CheckBookings()
        {
            Console.WriteLine(
                $"Booking for {Customer.Name} on {Date.ToShortDateString()} at {Time} — Status: {Status}");
        }

        public void MakeBooking(DateTime selectedDate, string selectedEmployee, TimeSpan selectedTime, string selectedService)
        {
            Date = selectedDate;
            Time = selectedTime;
            Employee = Employee.AllEmployees.FirstOrDefault(e => 
                e.Name.Equals(selectedEmployee, StringComparison.OrdinalIgnoreCase));

            Service = Service.AllServices.FirstOrDefault(e => 
                e.Name.Equals(selectedService, StringComparison.OrdinalIgnoreCase));

            if (Employee == null)
            {
                Console.WriteLine("Employee not found. Please choose a valid employee.");
                return;
            }

            if (Service == null)
            {
                Console.WriteLine("Service not found.");
                return;
            }

            Console.WriteLine($"You selected {Employee} on {Date.ToShortDateString()}.");

            if (!Customer.IsLoggedIn)
            {
                Console.WriteLine("You must log in to continue."); 
                return;
            }

            if (Customer.Age < Service.MinimalAge)
            {
                Console.WriteLine($"Booking failed: Minimum age for this service is {Service.MinimalAge}.");
                return;
            }

            ProcessPayment();

            Status = "accepted";
            Console.WriteLine($"Booking created successfully for {Customer.Name} with {Employee.Name}. Status: {Status}");

        }

        public bool ProcessPayment()
        {
            return true;
        }

        public void ConfirmBooking()
        {
            Status = "completed";
            Console.WriteLine("Booking confirmed and completed.");
        }

        public void ChangeBookingStatus(string newStatus)
        {
            if (new[] { "accepted", "completed", "canceled" }.Contains(newStatus))
            {
                Status = newStatus;
                Console.WriteLine($"Booking status changed to {newStatus}.");
            }
            else
            {
                Console.WriteLine("Invalid status value.");
            }
        }
    }
}
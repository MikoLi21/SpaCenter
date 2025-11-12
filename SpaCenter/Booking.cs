namespace SpaCenter;

public class Booking
{
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; private set; } // accepted, completed, canceled

        // Associations
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
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
            Console.WriteLine($"Booking for {Customer.Name} on {Date.ToShortDateString()} at {Time} — Status: {Status}");
        }

        public void MakeBooking()
        {
            Status = "accepted";
            Console.WriteLine("Booking created successfully.");
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
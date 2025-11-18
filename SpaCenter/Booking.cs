using System;
using System.Collections.Generic;
using System.Linq;
namespace SpaCenter
{
    [Serializable]
    public class Booking
    {
        //Booking Container
        private static List<Booking> bookings_List = new List<Booking>();
        public static IReadOnlyList<Booking> Bookings => bookings_List.AsReadOnly();
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (value < DateTime.Today)
                {
                    throw new ArgumentException("Booking can't be planned on date earlier than today");
                }
                _date = value;
            }
        }
        public PaymentMethod PaymentMethod { get; set; }
        
        public BookingStatus Status { get; set; }
        
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public Service Service { get; set; }

        public Booking(Customer customer, Employee employee, Service service, DateTime date, PaymentMethod paymentMethod)
        {
            Customer = customer;
            Employee = employee;
            Service = service;
            Date = date;
            PaymentMethod = paymentMethod;
            Status = BookingStatus.Accepted;
            
            addBooking(this);
        }
        
        private static void addBooking(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentException("Booking cannot be null");
            }
            bookings_List.Add(booking);
        }
        
        public static void LoadExtent(IEnumerable<Booking>? list)
        {
            bookings_List.Clear();

            if (list == null) return;

            bookings_List.AddRange(list);
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*public void UpdateBooking(DateTime newDate, TimeSpan newTime)
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
            if (Status != "accepted")
            {
                Console.WriteLine("Booking cannot be confirmed because it has not been accepted yet.");
                return;
            }

            Status = "completed";
            Console.WriteLine($"Booking confirmed and completed for {Customer.Name} with {Employee.Name}.");
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
        }*/
    }
}
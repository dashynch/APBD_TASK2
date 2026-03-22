using APBD_TASK2.Database;
using APBD_TASK2.Intercaces;
using APBD_TASK2.Models;
using APBD_TASK2.Enum;

namespace APBD_TASK2.Services
{

    public class RentalService : IRentalService
    {
        private const decimal PenaltyDay = 10m;
        private readonly Singleton _singleton = Singleton.Instance;
        
        public void AddEquipment(Equipment equipment)
        {
            _singleton.Add(equipment);
            Console.WriteLine($"Equipment {equipment.Name} added");
        }

        public void AddUser(User user)
        {
            _singleton.Add(user);
            Console.WriteLine($"User {user.Name} added");
        }

        public List<Equipment> GetAllEquipment()
        {
            return _singleton.Equipment;
        }

        public List<Equipment> GetAvailableEquipment()
        {
            return _singleton.Equipment.Where(e => e.Status == EquipmentStatus.Available).ToList();
        }

        public bool RentEquipment(int userId, int equipmentId, DateTime returnDate)
        {
            var user = _singleton.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine("User not found");
                return false;
            }
            
            var equipment = _singleton.Equipment.FirstOrDefault(e => e.Id == equipmentId);
            if (equipment == null)
            {
                Console.WriteLine("Equipment not found");
                return false;
            }
            if (equipment.Status != EquipmentStatus.Available)
            {
                Console.WriteLine($"Equipment {equipment.Name} is not available");
                return false;
            }

            var activeRentals = _singleton.Rentals.Count(r => r.User.Id == userId && !r.IsReturned);
            if (activeRentals >= user.MaxActiveRentals)
            {
                Console.WriteLine($"User {user.Name} has reached the maximum allowed rentals {user.MaxActiveRentals} ");
                return false;
            }

            var rental = new Rental(user, equipment, returnDate);
            equipment.Status = EquipmentStatus.Rented;
            _singleton.Rentals.Add(rental);
            Console.WriteLine($"Equipment {equipment.Name} rented to {user.Name} until {returnDate:yy-MM-dd}");
            return true;
        }

        public bool ReturnEquipment(int rentalId)
        {
            var rental = _singleton.Rentals.FirstOrDefault(r => r.Id == rentalId);
            if (rental == null)
            {
                Console.WriteLine("Rental not found");
                return false;
            }

            if (rental.IsReturned)
            {
                Console.WriteLine("Equipment is already returned");
                return false;
            }

            rental.ActualReturnDate = DateTime.Now;
            rental.Equipment.Status = EquipmentStatus.Available;
            if (rental.ActualReturnDate > rental.ReturnDate)
            {
                var lateDays = (rental.ActualReturnDate.Value - rental.ReturnDate).Days;
                rental.Penalty = lateDays * PenaltyDay;
                Console.WriteLine($"Equipment {rental.Equipment.Name} returned late by {lateDays} and Penalty: {rental.Penalty}");
            }
            else
            {
                Console.WriteLine($"Equipment {rental.Equipment.Name} returned on time");
            }

            return true;
        }

        public void EquipmentUnavailable(int equipmentId)
        {
            var equipment = _singleton.Equipment.FirstOrDefault(e => e.Id == equipmentId);
            if (equipment == null)
            {
                Console.WriteLine("Equipment not found");
                return;
            }

            equipment.Status = EquipmentStatus.Unavailable;
            Console.WriteLine($"Equipment {equipment.Name} unavailable");
        }

        public List<Rental> GetAvtiveRentals(int userId)
        {
            return _singleton.Rentals.Where(r => r.User.Id == userId && !r.IsReturned).ToList();
        }

        public List<Rental> GetReturnedRentals()
        {
            return _singleton.Rentals.Where(r => r.IsReturned).ToList();
        }

        public void PrintRentals()
        {
            Console.WriteLine($"Users in total: {_singleton.Users.Count}");
            Console.WriteLine($"Equipment in total: {_singleton.Equipment.Count}");
            Console.WriteLine($"Equipment available: {_singleton.Equipment.Count(e => e.Status == EquipmentStatus.Available)}");
            Console.WriteLine($"Equipment rented: {_singleton.Equipment.Count(e => e.Status == EquipmentStatus.Rented)}");
            Console.WriteLine($"Equipment unavailable: {_singleton.Equipment.Count(e => e.Status == EquipmentStatus.Unavailable)}");
            Console.WriteLine($"Rentals in total: {_singleton.Rentals.Count}");
            Console.WriteLine($"Rentals active: {_singleton.Rentals.Count(r => !r.IsReturned)}");
            Console.WriteLine($"Rentals returned: {_singleton.Rentals.Count(r => r.IsReturned)}");
            
        }
    }
    
    
    
}
using APBD_TASK2.Models;

namespace APBD_TASK2.Intercaces
{

    public interface IRentalService
    {
        void AddUser(User user);
        void AddEquipment(Equipment equipment);
        List<Equipment> GetAllEquipment();
        List<Equipment> GetAvailableEquipment();

        bool RentEquipment(int userId, int equipmentId, DateTime returnDate);
        bool ReturnEquipment(int rentalId);
        void EquipmentUnavailable(int equipmentId);
        List<Rental> GetAvtiveRentals(int userId);
        List<Rental> GetReturnedRentals();
        void PrintRentals();
    }
}
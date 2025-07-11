namespace RentAutoApp.GCommon.Enums;

public enum ReservationStatus
{
    Booked = 1, // the car is reserved, but not yet picked up.
    Confirmed = 2, // the car is confirmed for pickup and ready for the user.
    Cancelled = 3, // the reservation has been cancelled by the user or the system.
    Pending = 4, // the reservation is awaiting confirmation or further action.
    Expired = 5, // the reservation has expired without being picked up.
    PickedUp = 6, // the reservation is currently active, meaning the user has picked up the vehicle.
    Returned = 7, // the vehicle has been returned after the reservation period.
    Overdue = 8, // the vehicle was not returned on time, indicating a delay.
    NoShow = 9 // the user did not show up for the reservation, and it was not cancelled.
}

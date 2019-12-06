using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Interfaces
{
    public interface IHotelService
    {
        Response<string> Create(int FloorNumber,int RoomAmount);
        Response<string> Book(string RoomNumber, string GuestName, int Age);
        Response<string> BookByFloor(int FloorNumber, string GuestName, int Age);
        Response<string> AvailableRooms();
        Response<string> Checkout(int KeyCard,string GuestName);
        Response<string> CheckoutGuestByFloor(int FloorNumber);
        Response<string> Guests();
        Response<string> GuestInRoom(string RoomNumbers);
        Response<string> GuestsByAge(string Operand,int Age);
        Response<string> GuestsByFloor(int Floor);
    }
}

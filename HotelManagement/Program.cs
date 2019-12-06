using HotelManagement.Interfaces;
using HotelManagement.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    class Program
    {
        public IHotelService hotelService;
        public Program()
        {
            hotelService = new HotelService();
        }

        static void Main(string[] args)
        {
            new Program().commandReader();
            Console.ReadLine();
        }

        public void commandReader()
        {
            try
            {
                string textFile = AppDomain.CurrentDomain.BaseDirectory + "//input.txt";
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    var command = line.Split(' ');
                    switch (command[0])
                    {
                        case "create_hotel":
                            {
                                int FloorNumber = int.Parse(command[1]);
                                int RoomAmount = int.Parse(command[2]);
                                var resp = hotelService.Create(FloorNumber, RoomAmount);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "book":
                            {
                                string RoomNumber = command[1];
                                string GuestName = command[2];
                                int Age = int.Parse(command[3]);
                                var resp = hotelService.Book(RoomNumber, GuestName, Age);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "list_available_rooms":
                            {
                                var resp = hotelService.AvailableRooms();
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "checkout":
                            {
                                int Keycard = int.Parse(command[1]);
                                string GuestName = command[2];
                                var resp = hotelService.Checkout(Keycard, GuestName);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "list_guest":
                            {
                                var resp = hotelService.Guests();
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "get_guest_in_room":
                            {
                                string RoomNumber = command[1];
                                var resp = hotelService.GuestInRoom(RoomNumber);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "list_guest_by_age":
                            {
                                string Operand = command[1];
                                int Age = int.Parse(command[2]);
                                var resp = hotelService.GuestsByAge(Operand, Age);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "list_guest_by_floor":
                            {
                                int FloorNumber = int.Parse(command[1]);
                                var resp = hotelService.GuestsByFloor(FloorNumber);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "checkout_guest_by_floor":
                            {
                                int FloorNumber = int.Parse(command[1]);
                                var resp = hotelService.CheckoutGuestByFloor(FloorNumber);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        case "book_by_floor":
                            {
                                int FloorNumber = int.Parse(command[1]);
                                string GuestName = command[2];
                                int Age = int.Parse(command[3]);
                                var resp = hotelService.BookByFloor(FloorNumber,GuestName,Age);
                                Console.WriteLine(resp.Result);
                                break;
                            }
                        default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR!!!!!");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

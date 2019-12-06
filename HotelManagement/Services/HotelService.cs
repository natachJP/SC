using HotelManagement.Interfaces;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services
{
    public class HotelService : IHotelService
    {
        public List<HotelModels> hotels;
        public List<int> keycards;

        public HotelService()
        {
            hotels = new List<HotelModels>();
            keycards = new List<int>();
        }

        public Response<string> AvailableRooms()
        {
            try
            {
                var result = hotels.Where(c => string.IsNullOrEmpty(c.GuestName));

                if (result.Count() != 0)
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = string.Join(",", result.Select(x => x.RoomNumber))
                    };
                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this hotel does not have any rooms available."
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> Book(string RoomNumber, string GuestName, int Age)
        {
            try
            {
                var hotel = hotels.FirstOrDefault(c => c.RoomNumber == RoomNumber);
                if (hotel != null)
                {
                    if (string.IsNullOrEmpty(hotel.GuestName))
                    {

                        hotel.GuestName = GuestName;
                        hotel.Age = Age;
                        hotel.Keycard = keycards.Min();
                        keycards.Remove(hotel.Keycard);

                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Room " + RoomNumber + " is booked by " + GuestName + " with keycard number " + hotel.Keycard + "."
                        };
                    }
                    else
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Cannot book room " + RoomNumber + " for " + GuestName + ", The room is currently booked by " + hotel.GuestName + "."
                        };
                    }
                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this room number is not found"
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> BookByFloor(int FloorNumber, string GuestName, int Age)
        {
            try
            {
                var hotel = hotels.Where(c => c.FloorNumber == FloorNumber);

                if (hotel.Count() != 0)
                {
                    var roomHasGuest = hotel.Where(c => !string.IsNullOrEmpty(c.GuestName));
                    if (roomHasGuest.Count() != 0)
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Cannot book floor "+FloorNumber+" for "+GuestName+"."
                        };
                    }
                    else
                    {
                        hotel = hotel.Select(x =>
                        {
                            x.GuestName = GuestName;
                            x.Age = Age;
                            x.Keycard = keycards.Min();
                            keycards.Remove(x.Keycard);
                            return x;
                        }).ToList();

                        string rooms = string.Join(",", hotel.Select(x => x.RoomNumber));
                        string keycard = string.Join(",", hotel.Select(x => x.Keycard));
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Room " + rooms + " are booked with keycard number " + keycard
                        };
                    }
                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this floor number is not found."
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> Checkout(int KeyCard, string GuestName)
        {
            try
            {
                var hotel = hotels.FirstOrDefault(c => c.Keycard == KeyCard);

                if (hotel != null)
                {

                    if (hotel.GuestName == GuestName)
                    {
                        keycards.Add(hotel.Keycard);
                        hotel.GuestName = string.Empty;
                        hotel.Keycard = 0;
                        hotel.Age = 0;

                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Room " + hotel.RoomNumber + " is checkout."
                        };
                    }
                    else
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Only " + hotel.GuestName + " can checkout with keycard number " + hotel.Keycard + "."
                        };
                    }

                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "Wrong Keycard."
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> CheckoutGuestByFloor(int FloorNumber)
        {
            try
            {
                var hotel = hotels.Where(c => c.FloorNumber == FloorNumber);

                if (hotel.Count() != 0)
                {
                    hotel = hotel.Where(c => !string.IsNullOrEmpty(c.GuestName));
                    if (hotel.Count() != 0)
                    {
                        string rooms = string.Join(",", hotel.Select(x => x.RoomNumber));

                        hotel = hotel.Select(x =>
                        {
                            keycards.Add(x.Keycard);
                            x.Age = 0;
                            x.Keycard = 0;
                            x.GuestName = string.Empty;
                            return x;
                        }).ToList();

                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "Room " + rooms + " is checkout."
                        };
                    }
                    else
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "this floor does not have any guests."
                        };
                    }

                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this hotel does not have "+FloorNumber+" floor."
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> Create(int FloorNumber, int RoomAmount)
        {
            try
            {

                for (int f = 1; f <= FloorNumber; f++)
                {
                    for (int r = 1; r <= RoomAmount; r++)
                    {
                        hotels.Add(new HotelModels()
                        {
                            FloorNumber = f,
                            RoomNumber = f.ToString() + r.ToString("00"),
                        });
                        keycards.Add(keycards.Count + 1);
                    }
                }

                return new Response<string>()
                {
                    IsSuccess = true,
                    Result = "Hotel created with " + FloorNumber + " floor(s), " + RoomAmount + " room(s) per floor."
                };
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public Response<string> GuestInRoom(string RoomNumber)
        {
            try
            {
                var result = hotels.FirstOrDefault(c => c.RoomNumber == RoomNumber);

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.GuestName))
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = result.GuestName
                        };
                    }
                    else
                    {
                        return new Response<string>()
                        {
                            IsSuccess = true,
                            Result = "this room does not have a guest."
                        };
                    }

                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this room number is not found"
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> Guests()
        {
            try
            {
                var result = hotels.Where(c => !string.IsNullOrEmpty(c.GuestName));

                if (result.Count() != 0)
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = string.Join(",", result.Select(x => x.GuestName).Distinct())
                    };
                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this hotel does not have any guests."
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> GuestsByAge(string Operand, int Age)
        {
            try
            {
                var result = hotels.Where(c => !string.IsNullOrEmpty(c.GuestName));

                if (result.Count() != 0)
                {
                    switch (Operand)
                    {
                        case "<":
                            {
                                result = result.Where(c => c.Age < Age);
                                return new Response<string>()
                                {
                                    IsSuccess = true,
                                    Result = result.Count() != 0 ? string.Join(",", result.Select(x => x.GuestName).Distinct()) : "this hotel does not have any guests younger than " + Age + "."
                                };
                                break;
                            }
                        case ">":
                            {
                                result = result.Where(c => c.Age > Age);
                                return new Response<string>()
                                {
                                    IsSuccess = true,
                                    Result = result.Count() != 0 ? string.Join(",", result.Select(x => x.GuestName).Distinct()) : "this hotel does not have any guests over than " + Age + "."
                                };
                                break;
                            }
                        case "=":
                            {
                                result = result.Where(c => c.Age == Age);
                                return new Response<string>()
                                {
                                    IsSuccess = true,
                                    Result = result.Count() != 0 ? string.Join(",", result.Select(x => x.GuestName).Distinct()) : "this hotel does not have any guests equal " + Age + "."
                                };
                                break;
                            }
                        default:
                            {
                                return new Response<string>()
                                {
                                    IsSuccess = true,
                                    Result = "Operand is no match in case"
                                };
                            }
                    }


                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this hotel does not have any guests."
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> GuestsByFloor(int Floor)
        {
            try
            {
                var result = hotels.Where(c => !string.IsNullOrEmpty(c.GuestName) && c.FloorNumber == Floor);

                if (result.Count() != 0)
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = string.Join(",", result.Select(x => x.GuestName).Distinct())
                    };
                }
                else
                {
                    return new Response<string>()
                    {
                        IsSuccess = true,
                        Result = "this hotel does not have any guests in "+Floor+" floor."
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

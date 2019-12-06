using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
    }
}

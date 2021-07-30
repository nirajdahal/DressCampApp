using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class APIResponse
    {
        public APIResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(StatusCode);
        }


        public int StatusCode { get; set; }

        public string Message { get; set; }

        private string GetDefaultMessage(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "You have made a BadRequest",
                401 => "Sorry you are not authorized ",
                404 => "THe resource you are looking for cannot be found",
                500 => "Something has gone wrong on the server",

                _ => null
            };
            return message;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserBasketService
    {
        Task<string> GetBasketIdForUser(string email);

        Task RemoveBasketIdFromUser(string email);
    }
}

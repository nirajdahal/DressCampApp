using Core.Dtos.Basket;
using Core.Entities.Basket;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserBasketService : IUserBasketService
    {
       
        private readonly UserManager<User> _userManager;
      
        public UserBasketService( UserManager<User> userManager)
        {
         
            _userManager = userManager;

        }


        public async Task<string> GetBasketIdForUser(string email)
        {
            

            var user = await _userManager.FindByEmailAsync(email);
          
            var basketId = user.BasketId;
          
            if(basketId == null)
            {
                var newBasketId = Guid.NewGuid().ToString();
                user.BasketId = newBasketId;

                await _userManager.UpdateAsync(user);
                basketId = newBasketId;
                
            }

            return basketId;

            

            //if user doesnot have basket id - create one 
        }
   
        public async Task RemoveBasketIdFromUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            user.BasketId = null;
            await _userManager.UpdateAsync(user);

        }
    
    }
}

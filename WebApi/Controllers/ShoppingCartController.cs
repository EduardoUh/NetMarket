using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class ShoppingCartController(IShoppingCartRepository shoppingCartRepository) : BaseApiController
    {
        private readonly IShoppingCartRepository _shoppingCartRepository = shoppingCartRepository;

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string id)
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync(id);

            // if shopping cart is null (doesn't exist) the return a shopping cart but with only the provided id
            return Ok(shoppingCart ?? new ShoppingCart(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateShoppingCart([FromBody] ShoppingCart shoppingCart)
        {
            var updatedShoppingCart = await _shoppingCartRepository.UpdateShoppingCartAsync(shoppingCart);

            return Ok(updatedShoppingCart);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteShoppingCart(string id)
        {
            return await _shoppingCartRepository.DeleteShoppingCartAsync(id);
        }

    }
}

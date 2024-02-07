using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace BusinessLogic.Data
{
    public class ShoppingCartRepository(IConnectionMultiplexer redis) : IShoppingCartRepository
    {
        // setting up the object that will manage the redis db
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<ShoppingCart> GetShoppingCartAsync(string id)
        {
            // getting the shopping car from the redis db
            var data = await _database.StringGetAsync(id);
            // parsing it to ShoppingCart instance
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data);
        }

        public async Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            // this method sets the key to hold the value. If ket already holds a value, it's overwritten, regardless of its type
            // the return value is a bool
            var status = await _database.StringSetAsync(shoppingCart.Id, JsonSerializer.Serialize(shoppingCart), TimeSpan.FromDays(30));

            if (!status) return null;

            return await GetShoppingCartAsync(shoppingCart.Id);
        }

        public async Task<bool> DeleteShoppingCartAsync(string id)
        {
            // return value is a bool
            return await _database.KeyDeleteAsync(id);
        }

    }
}

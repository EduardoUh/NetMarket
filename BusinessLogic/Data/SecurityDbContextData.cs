using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Data
{
    public class SecurityDbContextData
    {
        public static async Task SeedUserAsync(UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var user = new User
                    {
                        Name = "Eduardo",
                        LastName = "Uh",
                        UserName = "EduardoUh",
                        Email = "eduardoivanuhgamino@gmail.com",
                        Address = new Address
                        {
                            City = "Tekax",
                            Apartment = "La hermita",
                            ZipCode = "97970",
                            Street = "La hermina 452"
                        }
                    };

                    var result = await userManager.CreateAsync(user, "@Eduardo12345");

                    var logger = loggerFactory.CreateLogger<SecurityDbContextData>();

                    logger.LogInformation("User created successfully");
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<SecurityDbContextData>();
                logger.LogError(ex.Message, "Task to add a user Failed");
            }
        }

    }
}

using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using WebApi.Dtos;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // adding unit of work service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // adding token service
            services.AddScoped<ITokenService, TokenService>();

            // setting up Identity Core
            // builder is an instance of identity core (representation of the model)
            var builder = services.AddIdentityCore<User>();
            // adding services for the user type, it needs it to buil the tables
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<SecurityDbContext>();
            // adding user manager allow us to use that object to perform transactions over the security tables
            builder.AddSignInManager<SignInManager<User>>();

            // adding authentication, based on jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]!)),
                        ValidIssuer = Configuration["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(MappingProfiles));
            // we must add the typeof method since the interface and its implementation work with generics
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericSecurityRepository<>), typeof(GenericSecurityRepository<>));

            services.AddDbContext<NetMarketDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<SecurityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SecurityIdentityConnection"));
            });

            // adding redis service
            var redisConfiguration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis")!, true);
            redisConfiguration.AbortOnConnectFail = false;
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

            // adding service for the time in which records are created or updated
            services.TryAddSingleton<ISystemClock, SystemClock>();

            // this way we are adding a object of type IProductRepository with an implementation type
            // specified in ProductRepository to the specified IServiceColleciton
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddControllers();

            // adding cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsRule", rule =>
                {
                    rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* No longer neccessary because we do this in the ExceptionMiddleware class
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            */

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");

            app.UseRouting();

            // implementing the cors config
            app.UseCors("CorsRule");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}

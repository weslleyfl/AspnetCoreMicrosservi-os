using System;
using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection
               (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));


            var query = "SELECT * FROM Coupon WHERE ProductName ILIKE CONCAT('%',@ProductName,'%');";

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                sql: query,
                param: new
                {
                    ProductName = productName
                });

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
            }

            return coupon;

        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
               (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));


            var affected = await connection.ExecuteAsync(
                sql: "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                param: new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection
               (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));


            var affected = await connection.ExecuteAsync(
                sql: "DELETE FROM Coupon WHERE ProductName = @ProductName",
                 param: new
                 {
                     ProductName = productName
                 });


            return affected != 0;
        }


        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
               (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));


            var affected = await connection.ExecuteAsync(
                sql: "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                 param: new
                 {
                     ProductName = coupon.ProductName,
                     Description = coupon.Description,
                     Amount = coupon.Amount,
                     Id = coupon.Id

                 });

            return affected != 0;

        }
    }
}

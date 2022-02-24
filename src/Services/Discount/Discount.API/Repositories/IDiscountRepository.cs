using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountAsync(string productName);
        Task<bool> CreateDiscountAsync(Coupon coupon);
        Task<bool> UpdateDiscountAsync(Coupon coupon);
        Task<bool> DeleteDiscountAsync(string productName);

    }
}

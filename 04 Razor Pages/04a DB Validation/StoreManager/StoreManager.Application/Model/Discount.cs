using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Model
{
    public class Discount
    {
        public Discount(Product product, bool allowed, int? count = null, decimal? maxValue = null)
        {
            Product = product;
            ProductEan = product.Ean;
            Allowed = allowed;
            Count = count;
            MaxValue = maxValue;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Discount() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int Id { get; private set; }
        public int ProductEan { get; set; }
        public Product Product { get; set; }
        public bool Allowed { get; set; }
        public int? Count { get; set; }
        public decimal? MaxValue { get; set; }
    }
}

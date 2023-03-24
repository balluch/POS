using StoreManager.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Dto
{
    class ValidProduct : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var discountDto = validationContext.ObjectInstance as DiscountDto;
            if (discountDto is null) { return null; }

            var db = validationContext.GetService(typeof(StoreContext)) as StoreContext;

            var offer = db?.Offers.FirstOrDefault(o => o.Product.Guid == discountDto.ProductGuid && o.Store.Guid == discountDto.StoreGuid);
            if (offer is null)
                return new ValidationResult("Angebot für dieses Produkt nicht vorhanden.");

            var discount = db?.Discounts.FirstOrDefault(d => d.Product.Guid == discountDto.ProductGuid && d.Allowed);
            if (discount is null)
                return new ValidationResult("Rabatt darf für dieses Produkt nicht gewährt werden.");

            return ValidationResult.Success;
        }
    }
    class ValidDiscount : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var discountDto = validationContext.ObjectInstance as DiscountDto;
            if (discountDto is null) { return null; }

            var db = validationContext.GetService(typeof(StoreContext)) as StoreContext;

            var maxValue = db?.Discounts.FirstOrDefault(d => d.Product.Guid == discountDto.ProductGuid && d.Allowed)?.MaxValue;
            if (maxValue is not null && (discountDto.Discount < 0 || discountDto.Discount > maxValue))
                return new ValidationResult($"Der Rabatt muss zwischen 0 und {maxValue} liegen.");

            return ValidationResult.Success;
        }
    }
    public record DiscountDto(
        Guid StoreGuid,
        [ValidProduct]
        Guid ProductGuid,
        [ValidDiscount]
        decimal Discount);
}

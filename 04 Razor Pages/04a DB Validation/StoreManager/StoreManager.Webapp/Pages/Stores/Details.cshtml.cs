using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManager.Application.Dto;
using StoreManager.Application.Infrastructure;
using StoreManager.Application.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoreManager.Webapp.Pages.Stores
{
    public class DetailsModel : PageModel
    {
        private readonly StoreContext _db;
        private readonly IMapper _mapper;

        public DetailsModel(StoreContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Guid { get; set; }

        public OfferDto NewOffer { get; set; } = default!;
        public Store Store { get; private set; } = default!;
        public IReadOnlyList<Offer> Offers { get; private set; } = new List<Offer>();
        
        public IEnumerable<SelectListItem> ProductWithoutOfferSelectList =>
            _db.Products.Where(p => !Offers.Select(o => o.ProductEan).Contains(p.Ean)).OrderBy(p => p.Name).Select(p => new SelectListItem(p.Name, p.Guid.ToString()));

        public IEnumerable<SelectListItem> ProductSelectList =>
            _db.Products.OrderBy(p => p.Name).Select(p => new SelectListItem(p.Name, p.Guid.ToString()));

        public DiscountDto NewDiscount { get; set; } = default!;

        public IActionResult OnPostNewOffer(Guid guid, OfferDto newOffer)
        {
            ModelState.Remove("ProductGuid");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var offer = _mapper.Map<Offer>(newOffer);
                offer.Product = _db.Products.FirstOrDefault(p => p.Guid == newOffer.ProductGuid)
                    ?? throw new ApplicationException("Ungültiges Produkt.");
                offer.Store = _db.Stores.FirstOrDefault(s => s.Guid == guid)
                    ?? throw new ApplicationException("Ungültiger Store.");
                offer.LastUpdate = DateTime.UtcNow;
                _db.Offers.Add(offer);
                _db.SaveChanges();
            }
            catch (ApplicationException e)
            {
                ModelState.AddModelError("", e.Message);
                return Page();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Fehler beim Schreiben in die Datenbank.");
                return Page();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostNewDiscount(Guid guid, DiscountDto newDiscount)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var discount = _db.Discounts.FirstOrDefault(d => d.Product.Guid == newDiscount.ProductGuid && d.Allowed) 
                    ?? throw new ApplicationException("Ungültiger Rabatt.");
                discount.Count++;
                var offer = _db.Offers.FirstOrDefault(o => o.Product.Guid == newDiscount.ProductGuid && o.Store.Guid == guid) 
                    ?? throw new ApplicationException("Ungültiger Store.");
                offer.Price -= newDiscount.Discount;
                offer.LastUpdate = DateTime.Now;
                _db.SaveChanges();
            }
            catch (ApplicationException e)
            {
                ModelState.AddModelError("", e.Message);
                return Page();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Fehler beim Schreiben in die Datenbank.");
                return Page();
            }
            return RedirectToPage();
        }

        public IActionResult OnGet(Guid guid)
        {
            return Page();
        }

        // Filter: Wird vor jedem Handler ausgeführt
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // SELECT * FROM Stores INNER JOIN Offers ON (...)
            // INNER JOIN Product ON (...)
            var store = _db.Stores
                .FirstOrDefault(s => s.Guid == Guid);
            if (store is null)
            {
                context.Result = RedirectToPage("/Stores/Index");
                return;
            }
            Store = store;
            Offers = _db.Offers.Include(o => o.Product).Where(o => o.Store.Guid == Guid).ToList();
        }
    }
}

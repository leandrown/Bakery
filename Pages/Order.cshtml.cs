using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bakery.Data;
using Bakery.Models;

namespace Bakery.Pages
{
    public class OrderModel : PageModel
    {
       private BakeryContext db;
        public OrderModel(BakeryContext db) => this.db = db;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Product Product { get; set; }
        [BindProperty, EmailAddress, Required, Display(Name = "Your e-mail address")]
        public string OrderEmail { get; set; }
        [BindProperty, Required(ErrorMessage = "Please, supply a shipping address"), Display(Name = "Shipping address")]
        public string OrderShipping { get; set; }
        [BindProperty, Display(Name = "Quantity")]
        public int OrderQuantity { get; set; } = 1;
        public async Task OnGetAsync() => Product = await db.Products.FindAsync(Id);
        public async Task<IActionResult> OnPostAsync()
        {
           Product = await db.Products.FindAsync(Id);
           if (ModelState.IsValid)
           {
              return RedirectToPage("OrderSuccess");
           }
           return Page();
        }
    }
}

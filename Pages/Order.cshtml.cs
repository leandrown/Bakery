using System;
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
        public async Task OnGetAsync() => Product = await db.Products.FindAsync(Id);
    }
}

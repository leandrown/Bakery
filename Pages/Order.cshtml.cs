using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
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
              string body = $@"<p>Thank you, we have received your order for {OrderQuantity} unit(s) of {Product.Name}!</p>
              <p>Your address is: <br />{OrderShipping.Replace("\n", "<br />")}</p>
              Your total is ${Product.Price * OrderQuantity}.<br />
              We will contact you if we have questions about your order. Thanks!<br />";

              /*
              Local test email
              ---------------------------------------------------------------------
              using(SmtpClient smtp = new SmtpClient())
              {
                 smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                 smtp.PickupDirectoryLocation = @"d:\mailpickup";
                 MailMessage message = new MailMessage();
                 message.To.Add(OrderEmail);
                 message.Subject = "Fourth Coffee - New Order";
                 message.Body = body;
                 message.IsBodyHtml = true;
                 message.From = new MailAddress("sales@fourthcoffee.com");
                 await smtp.SendMailAsync(message);
              }
              -------------------------------------------------------------------- */

              using(SmtpClient smtp = new SmtpClient())
              {
                 NetworkCredential credential = new NetworkCredential
                 {
                    UserName = "contato@mandrillus.com.br",
                    Password = ""
                 };
                 smtp.Credentials = credential;
                 smtp.Host = "smtp-mail.outlook.com";
                 smtp.Port = 587;
                 smtp.EnableSsl = true;
                 MailMessage message = new MailMessage();
                 message.To.Add(OrderEmail);
                 message.Subject = "Fourth Coffee - New Order";
                 message.Body = body;
                 message.IsBodyHtml = true;
                 message.From = new MailAddress("contato@mandrillus.com.br");
                 await smtp.SendMailAsync(message);
              }
              return RedirectToPage("OrderSuccess");
           }
           return Page();
        }
    }
}

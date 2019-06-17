using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public InvoiceController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public ActionResult<ICollection<Invoice>> GetInvoice()
        {
            var item = databaseContext.invoices.Include(i => i.invoiceDetails).ToList();

            return item;
        }

        [HttpGet("{id}")]
        public ActionResult<Invoice> GetInvoiceItem(int id)
        {
            var item = databaseContext.invoices.Include(i => i.invoiceDetails).Where(e => e.id == id);

            if(item != null)
            {
                return item.First();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoiceItem(Invoice invoice)
        {
            databaseContext.invoices.Add(invoice);
            await databaseContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoiceItem), new { id = invoice.id }, invoice);
        }
    }
}
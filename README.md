# asp.net-core-api-crud-pg

##Dependencies Install
1. Dapper
2. Npgsql
3. Npgsql.EntityFrameworkCore.PostgreSQL
4. Npgsql.EntityFrameworkCore.PostgreSQL.Design

##Console
select ***Tools*** -> ***NuGet Package Manager*** -> ***Package Manager Console***

```
PM> enable-migrations
PM> add-migration initial
PM> update-database
```

##Folder Models

*Invoice.cs*
```
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime inv_date { get; set; }
        public ICollection<InvoiceDetail> invoiceDetails { get; set; }
    }
}
```

*InvoiceDetail.cs*
```
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class InvoiceDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int Invoiceid { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }

    }
}

```

*DatabaseContext.cs*
```
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    }
}

```

##File Startup.cs
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<DatabaseContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
    );
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info { Title = "MyPOS", Version = "v1" });
        }
    );
}
```
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty;
    });

    app.UseHttpsRedirection();
    app.UseMvc();
}
```

###Controller
*InvoiceController.cs*
```
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
```

##Raw Query
```
databaseContext.invoices.FromSql("SELECT * FROM public.invoices").ToList();
```

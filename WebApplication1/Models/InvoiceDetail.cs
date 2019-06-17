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

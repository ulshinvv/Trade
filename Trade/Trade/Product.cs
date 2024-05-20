using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set;  }
        public Decimal Cost { get; set; }
        public int QuantityInStock { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual ProductCategory Category { get; set; }
    }
}

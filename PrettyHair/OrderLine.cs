using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PrettyHair
{
    public class OrderLine : IUi
    {
        public ProductType Product { get; set; }
        public int Quantity { get; set; }

        public OrderLine() { }

        public OrderLine(ProductType product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return "\t" + Product.Description + " - " + Quantity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PrettyHair
{
    public class OrderLine
    {
        public List<int> OrderLinesProducts { get; set; }
        public List<int> OrderLinesQuantity { get; set; }

        public OrderLine()
        {
            OrderLinesProducts = new List<int>();
            OrderLinesQuantity = new List<int>();
        }

        internal void Add(List<int> productTypeId, List<int> quantity)
        {
            OrderLinesProducts.AddRange(productTypeId);
            OrderLinesQuantity.AddRange(quantity);
        }
    }
}

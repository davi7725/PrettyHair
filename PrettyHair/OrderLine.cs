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
        public List<int> orderLinesProducts { get; set; }
        public List<int> orderLinesQuantity { get; set; }

        public OrderLine()
        {
            orderLinesProducts = new List<int>();
            orderLinesQuantity = new List<int>();
        }

        internal void Add(List<int> productTypeId, List<int> quantity)
        {
            orderLinesProducts.AddRange(productTypeId);
            orderLinesQuantity.AddRange(quantity);
        }
    }
}

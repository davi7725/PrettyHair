using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class ProductType : IUi
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public ProductType()
        {

        }

        public ProductType(int id, double price, string description, int amount)
        {
            Id = id;
            Price = price;
            Description = description;
            Amount = amount;
        }

        public override string ToString()
        {
            return Id + " - " + Description + " - " + Price + " - " + Amount;
        }
    }
}

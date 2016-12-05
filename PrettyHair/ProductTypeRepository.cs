using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class ProductTypeRepository
    {
        Dictionary<int, ProductType> listOfProducts = new Dictionary<int, ProductType>();
        public void Clear()
        {
            listOfProducts.Clear();
        }

        public ProductType CreateProduct(int id, double price, string description, int amount)
        {
            ProductType product = new ProductType(id, price, description, amount);
            listOfProducts.Add(id, product);
            return product;
        }

        public int CountProducts()
        {
            return listOfProducts.Count;
        }

        public ProductType Load(int id)
        {
            return listOfProducts[id];
        }

        public void ChangePrice(double price, int id)
        {
            listOfProducts[id].Price = price;
        }

        public void ChangeAmount(int amount, int id)
        {
            listOfProducts[id].Amount = listOfProducts[id].Amount - amount;
        }
    }
}

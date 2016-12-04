using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class OrderRepository
    {
        Dictionary<int, Order> listOfOrders = new Dictionary<int, Order>();

        public void Clear()
        {
            listOfOrders.Clear();
        }

        public int CountOrders()
        {
            return listOfOrders.Count;
        }

        public Order InsertOrder(int customerId, DateTime date, DateTime deliveryDate, int id, int quantity)
        {
            Order order = new Order(id, date, deliveryDate, quantity, customerId);
            listOfOrders.Add(id, order);
            return order;
        }
    }
}

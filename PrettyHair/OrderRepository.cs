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

        public int CountOrders(int customerId)
        {
            int countOrders = 0;
            foreach (Order order in listOfOrders.Values)
            {
                if (order.CustomerId == customerId)
                {
                    countOrders++;
                }
            }
            return countOrders;
        }

        public Order InsertOrder(int customerId, DateTime date, DateTime deliveryDate, int orderId, List<int> quantity, List<int> productTypeId, ProductTypeRepository repoPr)
        {
            Order order = new Order(orderId, date, deliveryDate, productTypeId, quantity, customerId);
            for (int i = 0; i < productTypeId.Count; i++)
            {
                repoPr.ChangeAmount(quantity[i], productTypeId[i]);
            }
            listOfOrders.Add(orderId, order);
            return order;
        }

        public Order Load(int orderId)
        {
            return listOfOrders[orderId];
        }
    }
}

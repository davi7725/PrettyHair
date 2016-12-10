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
            listOfOrders.Add(orderId, order);
            return order;
        }

        public Order Load(int orderId)
        {
            return listOfOrders[orderId];
        }

        public void Register(int orderId, ProductTypeRepository repoPr)
        {
            for(int i = 0; i < listOfOrders[orderId].OrderLine.OrderLinesProducts.Count; i++)
            {
                int quantityToRemoveFromStock = listOfOrders[orderId].OrderLine.OrderLinesQuantity[i];
                repoPr.SubtractToAmount(quantityToRemoveFromStock, listOfOrders[orderId].OrderLine.OrderLinesProducts[i]);
            }
            listOfOrders[orderId].Registered = true;
        }

        public List<Order> CheckOrders(DateTime date, ProductTypeRepository repoPr)
        {
            List<Order> ordersOfThisDate = new List<Order>();
            foreach (Order ord in listOfOrders.Values)
            {
                if (date == ord.Date)
                {
                    if (repoPr.CheckAmountOfProductsInOrder(ord) == true)
                    {
                        ordersOfThisDate.Add(ord);
                    }
                    else
                    {
                        string email = BuildEmail(ord, repoPr);
                    }
                }

            }

            return ordersOfThisDate;
        }

        public string BuildEmail(Order ord, ProductTypeRepository repoPr)
        {
            string text = "Id:" + ord.Id + "\nProducts:";
            for (int i = 0; i < ord.OrderLine.OrderLinesProducts.Count; i++)
            {
                text = text + "\n" + ord.OrderLine.OrderLinesProducts[i] + " - " + repoPr.GetProductTypes()[ord.OrderLine.OrderLinesProducts[i]].Description + " - " + ord.OrderLine.OrderLinesQuantity + " - " + repoPr.GetProductTypes()[ord.OrderLine.OrderLinesProducts[i]].Amount;
            }
            return text;
        }
    }
}

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

        public event OnOrderReadyToPackHandler orderReadyToPack;
        public event OnAmountNotEnoughHandler orderAmountNotEnough;

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
                if (order.Customer.Id == customerId)
                {
                    countOrders++;
                }
            }
            return countOrders;
        }

        public Order InsertOrder(Customer customer, DateTime date, DateTime deliveryDate, int orderId, List<int> quantity, List<ProductType> productTypes, bool registered, ProductTypeRepository repoPr)
        {
            Order order = new Order(orderId, date, deliveryDate, productTypes, quantity, customer);
            order.Registered = registered;
            listOfOrders.Add(orderId, order);
            return order;
        }

        public Order Load(int orderId)
        {
            return listOfOrders[orderId];
        }

        public void Register(int orderId, ProductTypeRepository repoPr)
        {
            for(int i = 0; i < listOfOrders[orderId].ListOfOrderLines.Count; i++)
            {
                int quantityToRemoveFromStock = listOfOrders[orderId].ListOfOrderLines[i].Quantity;
                repoPr.SubtractToAmount(quantityToRemoveFromStock, listOfOrders[orderId].ListOfOrderLines[i].Product.Id);
            }
            listOfOrders[orderId].Registered = true;
        }

        public List<Order> CheckOrders(DateTime date, ProductTypeRepository repoPr)
        {
            List<Order> ordersOfThisDate = new List<Order>();
            foreach (Order ord in listOfOrders.Values)
            {
                if (date.ToString("yyyy-MM-dd") == ord.Date.ToString("yyyy-MM-dd"))
                {
                    if (repoPr.CheckAmountOfProductsInOrder(ord) == true)
                    {
                        ordersOfThisDate.Add(ord);
                        OnOrderReadyToPackEventArgs args = new OnOrderReadyToPackEventArgs();
                        string email = BuildEmailReadyToPackOrder(ord, repoPr);
                        args.EmailMessage = email;
                        if (orderReadyToPack != null)
                        {
                            orderReadyToPack(this, args);
                        }
                    }
                    else
                    {
                        string email = BuildEmailNotEnoughStock(ord, repoPr);
                        OnAmountNotEnoughEventArgs args = new OnAmountNotEnoughEventArgs();
                        args.EmailMessage = email;
                        if(orderAmountNotEnough != null)
                        {
                            orderAmountNotEnough(this, args);
                        }
                    }
                }

            }

            return ordersOfThisDate;
        }

        public string BuildEmailReadyToPackOrder(Order ord, ProductTypeRepository repoPr)
        {
            string text = "Id:" + ord.Id + "\nProducts:";
            for (int i = 0; i < ord.ListOfOrderLines.Count; i++)
            {
                text = text + "\n" + ord.ListOfOrderLines[i].Product.Id + " - " + repoPr.GetProductTypes()[ord.ListOfOrderLines[i].Product.Id].Description + " - " + ord.ListOfOrderLines[i].Quantity;
            }
            return text;
        }

        public string BuildEmailNotEnoughStock(Order ord, ProductTypeRepository repoPr)
        {
            string text = "Id:" + ord.Id + "\nProducts:";
            for (int i = 0; i < ord.ListOfOrderLines.Count; i++)
            {
                text = text + "\n" + ord.ListOfOrderLines[i].Product.Id + " - " + repoPr.GetProductTypes()[ord.ListOfOrderLines[i].Product.Id].Description + " - " + ord.ListOfOrderLines[i].Quantity + " - " + repoPr.GetProductTypes()[ord.ListOfOrderLines[i].Product.Id].Amount;
            }
            return text;
        }

        public int NewOrderNumber()
        {
            return listOfOrders.Count+1;
        }

        public Dictionary<int,Order> GetListOfOrders()
        {
            return listOfOrders;
        }

        public Dictionary<int, IUi> GetOrdersAsIUi()
        {
            Dictionary<int, IUi> dictionaryOfOrdersIUi = new Dictionary<int, PrettyHair.IUi>();
            foreach (KeyValuePair<int, Order> kvpOrder in listOfOrders)
            {
                dictionaryOfOrdersIUi.Add(kvpOrder.Key, (IUi)kvpOrder.Value);
            }

            return dictionaryOfOrdersIUi;
        }

        internal List<Order> GetListOfNonRegisteredOrders()
        {
            List<Order> listOfNonRegisteredOrders = new List<Order>();
            foreach(Order ord in listOfOrders.Values)
            {
                if(ord.Registered == false)
                {
                    listOfNonRegisteredOrders.Add(ord);
                }
            }
            return listOfNonRegisteredOrders;
        }
    }
}

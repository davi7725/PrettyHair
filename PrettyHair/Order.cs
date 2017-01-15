using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class Order : IUi
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Customer Customer { get; set; }
        public List<OrderLine> ListOfOrderLines { get; set; }
        public event OnOrderIsPackedHandler orderIsPacked;
        private bool _registered =false;

        public bool Registered
        {
            get
            {
                return _registered;
            }
            set
            {
                _registered = value;
                if (value == true)
                {
                    OnOrderIsPackedEventArgs args = new OnOrderIsPackedEventArgs();
                    args.ListOfOrderLines = ListOfOrderLines;
                    if(orderIsPacked != null)
                    {
                        orderIsPacked(this, args);
                    }
                }
            }
        }
        public Order() { }

        public Order(int id, DateTime date, DateTime deliveryDate, List<ProductType> productTypes, List<int> quantity, Customer customer)
        {
            Id = id;
            Date = date;
            DeliveryDate = deliveryDate;
            Customer = customer;
            ListOfOrderLines = new List<OrderLine>();
            for (int i = 0; i < productTypes.Count; i++)
            {
                OrderLine ol = new OrderLine(productTypes[i], quantity[i]);
                ListOfOrderLines.Add(ol);
            }
        }

        public Order(int id, DateTime date, DateTime deliveryDate, Customer customer)
        {
            Id = id;
            Date = date;
            DeliveryDate = deliveryDate;
            Customer = customer;
            ListOfOrderLines = new List<OrderLine>();
        }

        public string ToString(bool orderlines)
        {

            string returnString = "";

            if (orderlines == false)
            {
                returnString = Id + " - " + Customer.Name + " - " + Date.ToString("yyyy-MM-dd") + " - " + DeliveryDate.ToString("yyyy-MM-dd") + " - " + Registered;
            }
            return returnString;
        }
        public override string ToString()
        {
            string returnString = Id + " - " + Customer.Name + " - " + Date.ToString("yyyy-MM-dd") + " - " + DeliveryDate.ToString("yyyy-MM-dd") + " - " + Registered + "\n";

            for (int i = 0; i < ListOfOrderLines.Count; i++)
            {
                returnString += ListOfOrderLines[i].ToString() + "\n";
            }

            return returnString;
        }



    }
}

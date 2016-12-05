using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int CustomerId { get; set; }
        public OrderLine OrderLine { get; set; }
        public Order() { }
        public Order(int id, DateTime date, DateTime deliveryDate, List<int> productTypeId, List<int> quantity, int customerId)
        {
            Id = id;
            Date = date;
            DeliveryDate = deliveryDate;
            CustomerId = customerId;
            OrderLine = new OrderLine();
            OrderLine.Add(productTypeId, quantity);
        }
    }
}

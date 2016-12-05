using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class CustomerRepository
    {
        Dictionary<int, Customer> listOfCustomers = new Dictionary<int, Customer>();

        public void Clear()
        {
            listOfCustomers.Clear();
        }

        public int CountCustomers()
        {
            return listOfCustomers.Count;
        }

        public Customer Create(int id, string name, string address)
        {
            Customer customer = new Customer(id, name, address);
            listOfCustomers.Add(id, customer);
            return customer;
        }

        public Customer Load(int id)
        {
            return listOfCustomers[id];
        }

        public void Remove(int id)
        {
            listOfCustomers.Remove(id);
        }

        public void ChangeAddress(string newAddress, int id)
        {
            listOfCustomers[id].Address = newAddress;
        }
    }
}

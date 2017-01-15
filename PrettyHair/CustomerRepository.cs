using System.Collections.Generic;

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
            if (listOfCustomers.ContainsKey(id))
                return listOfCustomers[id];
            else
                return null;
        }

        public void Remove(int id)
        {
            listOfCustomers.Remove(id);
        }

        public void ChangeAddress(string newAddress, int id)
        {
            listOfCustomers[id].Address = newAddress;
        }

        public Dictionary<int, Customer> GetListOfCustomers()
        {
            return listOfCustomers;
        }

        public Dictionary<int, IUi> GetCustomersAsIUi()
        {
            Dictionary<int, IUi> dictionaryOfCustomersIUi = new Dictionary<int, PrettyHair.IUi>();
            foreach (KeyValuePair<int, Customer> kvpCustomer in listOfCustomers)
            {
                dictionaryOfCustomersIUi.Add(kvpCustomer.Key, (IUi)kvpCustomer.Value);
            }

            return dictionaryOfCustomersIUi;
        }


        public int NewCustomerId()
        {
            return listOfCustomers.Count + 1;
        }

    }
}

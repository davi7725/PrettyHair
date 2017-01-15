using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    class UI
    {

        public static void ShowMenu()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("1 - Insert Product");
            Console.WriteLine("2 - Insert Customer");
            Console.WriteLine("3 - Insert Order");
            Console.WriteLine("4 - Get all the products");
            Console.WriteLine("5 - Get all customers");
            Console.WriteLine("6 - Get all orders");
            Console.WriteLine("7 - Get product by id");
            Console.WriteLine("8 - Get customer by id");
            Console.WriteLine("9 - Change product price");
            Console.WriteLine("10 - Change product amount");
            Console.WriteLine("11 - Change product description");
            Console.WriteLine("12 - Change customer address");
            Console.WriteLine("13 - Get all orders for today");
            Console.WriteLine("14 - Register order as packed");
            Console.WriteLine("15 - Exit");
        }

        static public void PrintList(Dictionary<int, IUi> iUiObject)
        {
            foreach(IUi iUi in iUiObject.Values)
            {
                WriteL(iUi.ToString());
            }
        }

        static public void WriteL(string text)
        {
            Console.WriteLine(text);
        }

        static public void Write(string text)
        {
            Console.Write(text);
        }

        static public void Clear()
        {
            Console.Clear();
        }

        static public void Wait()
        {
            Console.ReadKey();
        }

        internal static void PrintListOfNonRegisteredOrders(List<Order> listOfOrders)
        {
            foreach(Order ord in listOfOrders)
            {
                WriteL(ord.ToString());
            }
        }
    }
}

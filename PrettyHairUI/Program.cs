using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrettyHair;
using System.Data.SqlClient;

namespace PrettyHairUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.Run();
        }

        ProductTypeRepository repoPr = new ProductTypeRepository();
        CustomerRepository repoCu = new CustomerRepository();
        OrderRepository repoOr = new OrderRepository();

        SqlConnection conn = new SqlConnection("Server=ealdb1.eal.local; Database=ejl71_db; User Id=ejl71_usr; Password=Baz1nga71");
        SqlConnection conn1 = new SqlConnection("Server=ealdb1.eal.local; Database=ejl71_db; User Id=ejl71_usr; Password=Baz1nga71");

        public void Run()
        {
            InitializeRepositories();
            Menu();

        }

        public void InitializeRepositories()
        {
            try
            {
                conn.Open();
                conn1.Open();
                InitializeProductTypeRepository();
                InitializeCustomerRepository();
                InitializeOrderRepository();
                
            }
            catch (SqlException e)
            {
                Console.WriteLine("UPS :( ..." + e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
                conn1.Close();
            }
        }

        public void InitializeProductTypeRepository()
        {
            repoPr.Clear();

            SqlCommand cmd = new SqlCommand("SP_GET_ALL_PRODUCTS", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    repoPr.CreateProduct(Convert.ToInt32(rdr["PRODUCT_ID"]), Convert.ToDouble(rdr["PRICE"]), rdr["DESCRIPTION"].ToString(), Convert.ToInt32(rdr["AMOUNT"]));
                }
            }
            rdr.Close();
        }

        public void InitializeCustomerRepository()
        {
            repoCu.Clear();
            SqlCommand cmd = new SqlCommand("SP_GET_ALL_CUSTOMERS",conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    repoCu.Create(Convert.ToInt32(rdr["CUSTOMER_ID"]), rdr["NAME"].ToString(), rdr["ADDRESS"].ToString());
                }
            }
            rdr.Close();
        }

        public void InitializeOrderRepository()
        {
            repoOr.Clear();
            SqlCommand cmd = new SqlCommand("SP_GET_ALL_ORDERS", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    SqlCommand cmd1 = new SqlCommand("SP_GET_ORDER_LINE_BY_ORDER_ID", conn1);
                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@ORDER_ID", rdr["ORDER_ID"]));

                    List<int> listOfOrderLinesQuantity = new List<int>();
                    List<int> listOfOrderLinesProducts = new List<int>();

                    SqlDataReader rdr1 = cmd1.ExecuteReader();

                    if (rdr1.HasRows)
                    {
                        while (rdr1.Read())
                        {
                            listOfOrderLinesProducts.Add(Convert.ToInt32(rdr1["PRODUCT_ID"]));
                            listOfOrderLinesQuantity.Add(Convert.ToInt32(rdr1["QUANTITY"]));
                        }
                        rdr1.Close();
                        cmd1.Dispose();
                    }

                    repoOr.InsertOrder(Convert.ToInt32(rdr["CUSTOMER_ID"]), Convert.ToDateTime(rdr["DATE"]), Convert.ToDateTime(rdr["DELIVERY_DATE"]), Convert.ToInt32(rdr["ORDER_ID"]), listOfOrderLinesQuantity, listOfOrderLinesProducts, repoPr);
                }
            }
            rdr.Close();
        }

        public void Menu()
        {
            bool isRunning = true;

            do
            {
                Console.Clear();
                InitializeRepositories();
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
                Console.WriteLine("14 - Exit");

                string input = Console.ReadLine();
                input = input.Trim();

                Console.Clear();

                switch (input)
                {
                    case "1":
                        InsertProduct();
                        break;
                    case "2":
                        InsertCustomer();
                        break;
                    case "3":
                        InsertOrder();
                        break;
                    case "4":
                        ShowListOfProducts();
                        Console.ReadKey();
                        break;
                    case "5":
                        ShowListOfCustomers();
                        Console.ReadKey();
                        break;
                    case "6":
                        ShowListOfOrders();
                        Console.ReadKey();
                        break;
                    case "7":
                        ShowProductById();
                        Console.ReadKey();
                        break;
                    case "8":
                        ShowCustomerById();
                        Console.ReadKey();
                        break;
                    case "9":
                        ChangeProductPrice();
                        Console.ReadKey();
                        break;
                    case "10":
                        ChangeProductAmount();
                        Console.ReadKey();
                        break;
                    case "11":
                        ChangeProductDescription();
                        Console.ReadKey();
                        break;
                    case "12":
                        ChangeCustomerAddress();
                        Console.ReadKey();
                        break;
                    case "13":
                        GetOrdersFromToday();
                        Console.ReadKey();
                        break;
                    case "14":isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Wrong option, please chose another one");
                        break;
                }
            } while (isRunning);
        }

        public void InsertProduct()
        {
            Console.Write("Product description:");
            string description = Console.ReadLine();

            Console.WriteLine("Product price:");
            float price = Convert.ToSingle(Console.ReadLine());

            Console.WriteLine("Product amount:");
            int amount = Convert.ToInt32(Console.ReadLine());

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_PRODUCTS", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", description));
                cmd.Parameters.Add(new SqlParameter("@PRICE", price));
                cmd.Parameters.Add(new SqlParameter("@AMOUNT", amount));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertCustomer()
        {
            Console.Write("Customer name:");
            string name = Console.ReadLine();

            Console.WriteLine("Customer address:");
            string address = Console.ReadLine();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_CUSTOMERS", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NAME", name));
                cmd.Parameters.Add(new SqlParameter("@ADDRESS", address));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertOrder()
        {
            Console.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Customer cust = repoCu.Load(customerId);

            if(cust == null)
            {
                Console.Clear();
                Console.WriteLine("Customer not found in the DB, please create one.");
                InsertCustomer();
                customerId = repoCu.NewCustomerId();
            }

            Console.WriteLine("Order date:");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Delivery date:");
            DateTime deliveryDate = Convert.ToDateTime(Console.ReadLine());

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_ORDERS", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CUSTOMER_ID", customerId));
                cmd.Parameters.Add(new SqlParameter("@DATE", date));
                cmd.Parameters.Add(new SqlParameter("@DELIVERY_DATE", deliveryDate));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
            Console.Clear();
            Console.Write("How many products are you going to buy?");
            int numberOfProducts = Convert.ToInt32(Console.ReadLine());

            int orderId = repoOr.NewOrderNumber();

            for(int i = 0; i<numberOfProducts; i++)
            {
                Console.Clear();
                ShowListOfProducts();
                Console.WriteLine("Choose a product:");
                int productId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Choose a quantity:");
                int productQuantity = Convert.ToInt32(Console.ReadLine());

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_INSERT_ORDER_LINE", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ORDER_ID", orderId));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", productId));
                    cmd.Parameters.Add(new SqlParameter("@QUANTITY", productQuantity));
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message.ToString());
                    Console.ReadKey();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void ShowListOfProducts()
        {
            Dictionary<int, ProductType> listOfProducts = repoPr.GetProductTypes();
            foreach (ProductType product in listOfProducts.Values)
            {
                Console.WriteLine(product.Id + " - " + product.Description + " - " + product.Price + " - " + product.Amount);
            }
        }

        public void ShowListOfCustomers()
        {

            Dictionary<int, Customer> listOfCustomers = repoCu.GetListOfCustomers();
            foreach (Customer cust in listOfCustomers.Values)
            {
                Console.WriteLine(cust.Id + " - " + cust.Name + " - " + cust.Address);
            }
        }

        public void ShowListOfOrders()
        {

            Dictionary<int, Order> listOfOrders = repoOr.GetListOfOrders();
            foreach (Order ord in listOfOrders.Values)
            {
                Console.WriteLine(ord.Id + " - " + repoCu.Load(ord.CustomerId).Name + " - " + ord.Date.ToString("yyyy-MM-dd") + " - " + ord.DeliveryDate.ToString("yyyy-MM-dd") + " - " + ord.Registered);
                for(int i = 0; i < ord.ListOfOrderLines.Count; i++)
                {
                    Console.WriteLine("\t" + repoPr.Load(ord.ListOfOrderLines[i].ProductId).Description + " - " + ord.ListOfOrderLines[i].Quantity);
                }
            }
        }

        public void ShowProductById()
        {
            Console.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            ProductType product = repoPr.Load(productId);

            Console.WriteLine(product.Id + " - " + product.Description + " - " + product.Price + " - " + product.Amount);

        }

        public void ShowCustomerById()
        {
            Console.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Customer cust = repoCu.Load(customerId);

            Console.WriteLine(cust.Id + " - " + cust.Name + " - " + cust.Address);

        }

        public void ChangeProductPrice()
        {
            ShowListOfProducts();

            Console.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New price:");
            float newPrice = Convert.ToSingle(Console.ReadLine());

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_CHANGE_PRODUCT_PRICE", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PRICE", newPrice));
                cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", productId));

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void ChangeProductAmount()
        {
            ShowListOfProducts();

            Console.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New amount:");
            int newAmount = Convert.ToInt32(Console.ReadLine());

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_CHANGE_PRODUCT_AMOUNT", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AMOUNT", newAmount));
                cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", productId));

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void ChangeProductDescription()
        {
            ShowListOfProducts();

            Console.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New amount:");
            string newDescription = Console.ReadLine();

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_CHANGE_PRODUCT_DESCRIPTION", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", newDescription));
                cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", productId));

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void ChangeCustomerAddress()
        {
            ShowListOfCustomers();

            Console.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New address:");
            string newAddress = Console.ReadLine();

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_CHANGE_CUSTOMER_ADDRESS", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ADDRESS", newAddress));
                cmd.Parameters.Add(new SqlParameter("@CUSTOMER_ID", customerId));

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void GetOrdersFromToday()
        {
            List<Order> listOfTodaysOrders = repoOr.CheckOrders(DateTime.Today, repoPr);
            foreach(Order ord in listOfTodaysOrders)
            {
                Console.WriteLine(ord.Id + " - " + ord.Date.ToString("yyyy-MM-dd") + " - " + ord.DeliveryDate.ToString("yyyy-MM-dd") + " - " + ord.CustomerId + " - " + ord.Registered);
            }
            Console.Write("Order id:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            ShowOrderLinesPerOrderId(orderId);

        }

        public void ShowOrderLinesPerOrderId(int orderId)
        {
            Order ord = repoOr.Load(orderId);
            foreach(OrderLine ol in ord.ListOfOrderLines)
            {
                Console.WriteLine(ol.ProductId + " - " + repoPr.Load(ol.ProductId).Description + " - " + ol.Quantity);
            }
        }
    }
}

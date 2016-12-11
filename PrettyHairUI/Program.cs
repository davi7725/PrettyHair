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

                repoPr.Clear();
                repoCu.Clear();
                repoOr.Clear();

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

                cmd.CommandText = "SP_GET_ALL_CUSTOMERS";
                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        repoCu.Create(Convert.ToInt32(rdr["CUSTOMER_ID"]), rdr["NAME"].ToString(), rdr["ADDRESS"].ToString());
                    }
                }
                rdr.Close();


                cmd.CommandText = "SP_GET_ALL_ORDERS";
                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        SqlCommand cmd1 = new SqlCommand("SP_GET_ORDER_LINE_BY_ORDER_ID", conn);
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

                        }

                        rdr1.Close();

                        repoOr.InsertOrder(Convert.ToInt32(rdr["CUSTOMER_ID"]), Convert.ToDateTime(rdr["DATE"]), Convert.ToDateTime(rdr["DELIVERY_DATE"]), Convert.ToInt32(rdr["ORDER_ID"]), listOfOrderLinesQuantity, listOfOrderLinesProducts, repoPr);
                    }
                }
                rdr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("UPS :( ..." + e.Message.ToString());
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
        }

        public void Menu()
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
            Console.WriteLine("14 - Exit");
            string input = Console.ReadLine();
            Console.Clear();

            switch(input)
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
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                case "8":
                    break;
                case "9":
                    break;
                case "10":
                    break;
                case "11":
                    break;
                case "12":
                    break;
                case "13":
                    break;
                default:
                    Console.WriteLine("Wrong option, please chose another one");
                    break;
            }
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

            DateTime date = DateTime.Today;

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
            foreach(ProductType product in listOfProducts.Values)
            {
                Console.WriteLine(product.Id + " - " + product.Description + " - " + product.Price);
            }
        }
    }
}

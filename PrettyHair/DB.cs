using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrettyHair
{
    public class DB
    {
        private static SqlConnection conn = new SqlConnection("Server=ealdb1.eal.local; Database=ejl71_db; User Id=ejl71_usr; Password=Baz1nga71");

        public static void ChangeProductPrice(int productId, float newPrice)
        {
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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ChangeProductAmount(int productId, int newAmount)
        {
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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void GetOrders(OrderRepository orderRepository,CustomerRepository customerRepository, ProductTypeRepository productTypeRepository)
        {
            List<Order> listOfOrders = new List<Order>();
            try
            {
                conn.Open();
                orderRepository.Clear();
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_ORDERS", conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        Customer customerOfThisOrder = customerRepository.Load(Convert.ToInt32(rdr["CUSTOMER_ID"]));
                        Order order = new Order(Convert.ToInt32(rdr["ORDER_ID"]), Convert.ToDateTime(rdr["DATE"]), Convert.ToDateTime(rdr["DELIVERY_DATE"]), customerOfThisOrder);
                        order.Registered = Convert.ToBoolean(rdr["Registered"]);
                        listOfOrders.Add(order);
                    }
                }

                rdr.Close();

                foreach (Order ord in listOfOrders)
                {
                    cmd = new SqlCommand("SP_GET_ORDER_LINE_BY_ORDER_ID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ORDER_ID", ord.Id));

                    List<int> listOfOrderLinesQuantity = new List<int>();
                    List<ProductType> listOfOrderLinesProducts = new List<ProductType>();

                    rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            listOfOrderLinesProducts.Add(productTypeRepository.Load(Convert.ToInt32(rdr["PRODUCT_ID"])));
                            listOfOrderLinesQuantity.Add(Convert.ToInt32(rdr["QUANTITY"]));
                        }
                        rdr.Close();
                        cmd.Dispose();
                    }

                    Order newOrder = orderRepository.InsertOrder(ord.Customer, ord.Date, ord.DeliveryDate, ord.Id, listOfOrderLinesQuantity, listOfOrderLinesProducts,ord.Registered, productTypeRepository);
                    ord.Customer.OrderRepository.InsertOrder(ord.Customer, ord.Date, ord.DeliveryDate, ord.Id, listOfOrderLinesQuantity, listOfOrderLinesProducts, ord.Registered, productTypeRepository);
                }
            }
            catch (SqlException e)
            {
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void GetCustomers(CustomerRepository customerRepository)
        {
            try
            {
                conn.Open();

                customerRepository.Clear();
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_CUSTOMERS", conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        customerRepository.Create(Convert.ToInt32(rdr["CUSTOMER_ID"]), rdr["NAME"].ToString(), rdr["ADDRESS"].ToString());
                    }
                }
            }
            catch (SqlException e)
            {
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void GetProductTypes(ProductTypeRepository productTypeRepository)
        {
            try
            {
                conn.Open();
                productTypeRepository.Clear();

                SqlCommand cmd = new SqlCommand("SP_GET_ALL_PRODUCTS", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        productTypeRepository.CreateProduct(Convert.ToInt32(rdr["PRODUCT_ID"]), Convert.ToDouble(rdr["PRICE"]), rdr["DESCRIPTION"].ToString(), Convert.ToInt32(rdr["AMOUNT"]));
                    }
                }
                rdr.Close();
            }
            catch (SqlException e)
            {
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }

        }



        public static void ChangeProductDescription(int productId, string newDescription)
        {

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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }
        public static void ChangeOrder(Order ord)
        {

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_CHANGE_ORDER", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ORDER_ID", ord.Id));
                cmd.Parameters.Add(new SqlParameter("@CUSTOMER_ID", ord.Customer.Id));
                cmd.Parameters.Add(new SqlParameter("@DATE", ord.Date));
                cmd.Parameters.Add(new SqlParameter("@DELIVERY_DATE", ord.DeliveryDate));
                cmd.Parameters.Add(new SqlParameter("@REGISTERED", ord.Registered));

                cmd.ExecuteNonQuery();
                
            }
            catch (SqlException e)
            {
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ChangeCustomerAddress(int customerId, string newAddress)
        {
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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void InsertOrder(int customerId, int orderId, DateTime date, DateTime deliveryDate, List<OrderLine> listOfOrderLines)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_ORDERS", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CUSTOMER_ID", customerId));
                cmd.Parameters.Add(new SqlParameter("@DATE", date));
                cmd.Parameters.Add(new SqlParameter("@DELIVERY_DATE", deliveryDate));
                cmd.ExecuteNonQuery();

                foreach (OrderLine ordLine in listOfOrderLines)
                {
                    cmd = new SqlCommand("SP_INSERT_ORDER_LINE", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ORDER_ID", orderId));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", ordLine.Product.Id));
                    cmd.Parameters.Add(new SqlParameter("@QUANTITY", ordLine.Quantity));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }


        public static void InsertProduct(string description, float price, int amount)
        {
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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void InsertCustomer(string name, string address)
        {

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
                UI.WriteL(e.Message.ToString());
                UI.Wait();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

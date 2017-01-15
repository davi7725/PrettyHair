using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Moq;

namespace PrettyHair
{
    public class Controller
    {
        ProductTypeRepository productTypeRepository = new ProductTypeRepository();
        CustomerRepository customerRepository = new CustomerRepository();
        OrderRepository orderRepository = new OrderRepository();

        
        

        public void CheckOrdersForToday()
        {
            orderRepository.CheckOrders(DateTime.Today, productTypeRepository);
        }
        public void InitializeRepositories()
        {
            try
            {
                InitializeProductTypeRepository();
                InitializeCustomerRepository();
                InitializeOrderRepository();
            }
            catch (SqlException e)
            {
                UI.WriteL("UPS :( ..." + e.Message.ToString());
                UI.Wait();
            }
        }

        private void InitializeProductTypeRepository()
        {
            DB.GetProductTypes(productTypeRepository);
        }

        private void InitializeCustomerRepository()
        {
            DB.GetCustomers(customerRepository);
        }

        private void InitializeOrderRepository()
        {
            DB.GetOrders(orderRepository, customerRepository,productTypeRepository);

            foreach(Order ord in orderRepository.GetListOfOrders().Values)
            {
                ord.orderIsPacked += SendEmailForPackedOrder;
            }
            orderRepository.orderReadyToPack += SendEmailForReadyToBePackedOrder;
            orderRepository.orderAmountNotEnough += SendEmailForNotEnoughtProductAmount;
        }

        public void SendEmailForPackedOrder(object o, OnOrderIsPackedEventArgs args)
        {
            if(productTypeRepository.CheckAmountOfProductsInOrder((Order)o) == true)
            {
                productTypeRepository.RemoveProductsFromOrder(args.ListOfOrderLines);
            }
        }
         
        public void SendEmailForReadyToBePackedOrder(object o, OnOrderReadyToPackEventArgs args)
        {
            var admin = new User() { UserName = "sist@eal.dk", Password = "!QAZ2wsx", NumMessagesCreated = 0 };
            var message = new Mail() { Content = args.EmailMessage };

            var mockMail = new Mock<IMailModule>();
            mockMail.Setup(x => x.SendMail(message, "someemail@test.com")).Callback(() => admin.NumMessagesCreated = 1);

            //Act
            mockMail.Object.SendMail(message, "someemail@test.com");

            if(admin.NumMessagesCreated == 0)
            {
                throw new Exception("COULD NOT SEND MAIL!");
            }
        }

        public void SendEmailForNotEnoughtProductAmount(object o, OnAmountNotEnoughEventArgs args)
        {
            var admin = new User() { UserName = "sist@eal.dk", Password = "!QAZ2wsx", NumMessagesCreated = 0 };
            var message = new Mail() { Content = args.EmailMessage };

            var mockMail = new Mock<IMailModule>();
            mockMail.Setup(x => x.SendMail(message, "someemail@test.com")).Callback(() => admin.NumMessagesCreated = 1);

            //Act
            mockMail.Object.SendMail(message, "someemail@test.com");

            if (admin.NumMessagesCreated == 0)
            {
                throw new Exception("COULD NOT SEND MAIL!");
            }
        }

        public void Menu()
        {
            bool isRunning = true;

            do
            {
                UI.Clear();
                InitializeRepositories();
                UI.ShowMenu();

                string input = Console.ReadLine();
                input = input.Trim();

                UI.Clear();

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
                        UI.Wait();
                        break;
                    case "5":
                        ShowListOfCustomers();
                        UI.Wait();
                        break;
                    case "6":
                        ShowListOfOrders();
                        UI.Wait();
                        break;
                    case "7":
                        ShowProductById();
                        UI.Wait();
                        break;
                    case "8":
                        ShowCustomerById();
                        UI.Wait();
                        break;
                    case "9":
                        ChangeProductPrice();
                        UI.Wait();
                        break;
                    case "10":
                        ChangeProductAmount();
                        UI.Wait();
                        break;
                    case "11":
                        ChangeProductDescription();
                        UI.Wait();
                        break;
                    case "12":
                        ChangeCustomerAddress();
                        UI.Wait();
                        break;
                    case "13":
                        GetOrdersFromToday();
                        UI.Wait();
                        break;
                    case "14":
                        RegisterOrderAsPacked();
                        break;
                    case "15":
                        isRunning = false;
                        break;
                    default:
                        UI.WriteL("Wrong option, please chose another one");
                        break;
                }
            } while (isRunning);
        }

        private void RegisterOrderAsPacked()
        {
            ShowListOfNonRegisteredOrders();
            UI.Write("Order Id: ");
            int id;
            if(Int32.TryParse(Console.ReadLine(), out id) == true)
            {
                Order ord = orderRepository.Load(id);
                ord.Registered = true;
                DB.ChangeOrder(ord);
            }

        }

        public void InsertProduct()
        {
            UI.Write("Product description:");
            string description = Console.ReadLine();

            UI.WriteL("Product price:");
            float price = Convert.ToSingle(Console.ReadLine());

            UI.WriteL("Product amount:");
            int amount = Convert.ToInt32(Console.ReadLine());

            DB.InsertProduct(description, price, amount);
        }

        public void InsertCustomer()
        {
            UI.Write("Customer name:");
            string name = Console.ReadLine();

            UI.WriteL("Customer address:");
            string address = Console.ReadLine();

            DB.InsertCustomer(name, address);
        }

        public void InsertOrder()
        {
            UI.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Customer cust = customerRepository.Load(customerId);

            if (cust == null)
            {
                UI.Clear();
                UI.WriteL("Customer not found in the DB, please create one.");
                InsertCustomer();
                customerId = customerRepository.NewCustomerId();
            }

            UI.WriteL("Order date:");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            UI.WriteL("Delivery date:");
            DateTime deliveryDate = Convert.ToDateTime(Console.ReadLine());

            UI.Clear();

            int orderId = orderRepository.NewOrderNumber();
            bool isChoosing = true;
            List<OrderLine> listOfOrderLines = new List<OrderLine>();
            do
            {
                UI.Clear();
                ShowListOfProducts();
                UI.WriteL("Choose a product (any char to exit):");
                int productId;

                isChoosing = Int32.TryParse(Console.ReadLine(), out productId);

                if (isChoosing == true)
                {
                    UI.WriteL("Choose a quantity:");
                    int productQuantity = Convert.ToInt32(Console.ReadLine());
                    listOfOrderLines.Add(new OrderLine(productTypeRepository.Load(productId), productQuantity));
                }

            } while (isChoosing == true);

            DB.InsertOrder(customerId, orderId, date, deliveryDate, listOfOrderLines);
            
        }

        public void ShowListOfProducts()
        {
            Dictionary<int, IUi> listOfProductsIUi = productTypeRepository.GetProductAsIUi();
            UI.PrintList(listOfProductsIUi);
        }

        public void ShowListOfCustomers()
        {

            Dictionary<int, IUi> listOfCustomersIUi = customerRepository.GetCustomersAsIUi();
            UI.PrintList(listOfCustomersIUi);
        }

        public void ShowListOfOrders()
        {

            Dictionary<int, IUi> listOfOrdersIUi = orderRepository.GetOrdersAsIUi();
            UI.PrintList(listOfOrdersIUi);
        }

        public void ShowListOfNonRegisteredOrders()
        {

            List<Order> listOfOrders = orderRepository.GetListOfNonRegisteredOrders();
            UI.PrintListOfNonRegisteredOrders(listOfOrders);
        }

        public void ShowProductById()
        {
            UI.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            ProductType product = productTypeRepository.Load(productId);

            UI.WriteL(product.Id + " - " + product.Description + " - " + product.Price + " - " + product.Amount);

        }

        public void ShowCustomerById()
        {
            UI.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Customer cust = customerRepository.Load(customerId);

            UI.WriteL(cust.Id + " - " + cust.Name + " - " + cust.Address);

        }

        public void ChangeProductPrice()
        {
            ShowListOfProducts();

            UI.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            UI.Write("New price:");
            float newPrice = Convert.ToSingle(Console.ReadLine());

            DB.ChangeProductPrice(productId, newPrice);
           
        }

        public void ChangeProductAmount()
        {
            ShowListOfProducts();

            UI.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            UI.Write("New amount:");
            int newAmount = Convert.ToInt32(Console.ReadLine());

            DB.ChangeProductAmount(productId, newAmount);
        }

        public void ChangeProductDescription()
        {
            ShowListOfProducts();

            UI.Write("Product id:");
            int productId = Convert.ToInt32(Console.ReadLine());

            UI.Write("New amount:");
            string newDescription = Console.ReadLine();

            DB.ChangeProductDescription(productId, newDescription);
        }

        public void ChangeCustomerAddress()
        {
            ShowListOfCustomers();

            UI.Write("Customer id:");
            int customerId = Convert.ToInt32(Console.ReadLine());

            UI.Write("New address:");
            string newAddress = Console.ReadLine();

            DB.ChangeCustomerAddress(customerId, newAddress);
        }

        public void GetOrdersFromToday()
        {
            List<Order> listOfTodaysOrders = orderRepository.CheckOrders(DateTime.Today, productTypeRepository);
            foreach (Order ord in listOfTodaysOrders)
            {
                UI.WriteL(ord.Id + " - " + ord.Date.ToString("yyyy-MM-dd") + " - " + ord.DeliveryDate.ToString("yyyy-MM-dd") + " - " + ord.Customer.Id + " - " + ord.Registered);
            }
            UI.Write("Order id:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            UI.Clear();
            ShowOrderLinesPerOrderId(orderId);


        }

        public void ShowOrderLinesPerOrderId(int orderId)
        {
            Order ord = orderRepository.Load(orderId);
            foreach (OrderLine ol in ord.ListOfOrderLines)
            {
                UI.WriteL(ol.Product.Id + " - " + productTypeRepository.Load(ol.Product.Id).Description + " - " + ol.Quantity);
            }
        }
    }
}

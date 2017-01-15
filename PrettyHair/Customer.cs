namespace PrettyHair
{
    public class Customer : IUi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public OrderRepository OrderRepository { get; set; }

        public Customer() {
            OrderRepository = new OrderRepository();
        }
        public Customer(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
            OrderRepository = new OrderRepository();
        }

        public override string ToString()
        {
            return Id + " - " + Name + " - " + Address;
        }
    }
}

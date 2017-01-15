namespace PrettyHair
{
    public enum Rights
    {
        Full,
        None
    }
    public class User
    {
        
        public User()
        {
            Rights = Rights.None;
        }

        public string ViewAllEmployees()
        {
            return "Here is the list";
        }

        public string Password { get; set; }
        public string UserName { get; set; }
        public Rights Rights { get; set; }
        public int NumMessagesCreated { get; internal set; }
    }
}
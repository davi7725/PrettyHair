using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyHair
{
    public class MailBox
    {
        List<Mail> mailList = new List<Mail>();
        public int NumReceivedMessages { get; set; }
        public void Add(Mail obj)
        {
            mailList.Add(obj);
            NumReceivedMessages++;
        }

        internal string GetLatestMessageText()
        {
            return mailList.Last().Content;
        }
    }
}

using System.Collections.Generic;

namespace PrettyHair
{
    public interface IMailModule
    {
        void SendMail(Mail mail, string sender);
        Mail GetEmailById(int id);
        void AddToDraft(Mail mail);
        void AddToSentMessages(Mail mail);
        void AddToSentSpam(Mail mail);
        List<Mail> GetAllMail();
    }
}

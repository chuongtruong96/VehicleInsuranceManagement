using Project3.Models;

namespace Project3.ModelsView
{
    public interface IMailService
    {
        bool SendMail(ContactUs mailData);
    }
}

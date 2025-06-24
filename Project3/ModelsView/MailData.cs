using System.ComponentModel;

namespace Project3.ModelsView
{
    public class MailData
    {
        
        [DisplayName("Address  Receiver ")]
        public string ReceiverEmail { get; set; }
        [DisplayName("Name  Receiver")]
        public string ReceiverName { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Body")]
       public string Body { get; set; }  
        public int Id { get; set; }
        public string? Topic { get; set; }
        public string? Message { get; set; }
        public string? FullName { get; set; }

        public bool? IsReplied { get; set; }
    }
}

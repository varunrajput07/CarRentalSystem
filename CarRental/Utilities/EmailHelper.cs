using System.Net;
using System.Net.Mail;

namespace CarRental.Utilities
{
    public interface IEmailHelper
    {
     void SendEmail(string htmlString, string receiverEmail);
       
    }
    public class EmailHelper : IEmailHelper
    {
        public IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(string htmlString, string receiverEmail)
        {
            string fromAddress = _configuration.GetSection("Smtp").GetSection("FromAddress").Value;
            string Server = _configuration.GetSection("Smtp").GetSection("Server").Value;
            string Port = _configuration.GetSection("Smtp").GetSection("Port").Value;
            string password = _configuration.GetSection("Smtp").GetSection("Password").Value;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(fromAddress);
                message.To.Add(new MailAddress(receiverEmail));
                message.Subject = "CarRental";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = Convert.ToInt32(Port);
                smtp.Host = Server;//"smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromAddress, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }

       
    }
}

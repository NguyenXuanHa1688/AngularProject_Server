using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public IActionResult SendEmail(Email request)
        {
            var email = new MimeMessage();
            var existedAcc = _context.UserDto;
            foreach (var acc in existedAcc)
            {
                if (acc.UserName == request.userName)
                {
                    email.From.Add(MailboxAddress.Parse("BillGoodboys169@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(request.email));
                    email.Subject = "HERE IS YOUR PASSWORD";
                    email.Body = new TextPart(TextFormat.Html) { Text = acc.Password };

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("BillGoodboys169@gmail.com", "cqaqcaxynqzkfpub");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return Ok();
                }

            }
            return BadRequest("NO USER MATCH THE DATABASE");
        }
    }
}

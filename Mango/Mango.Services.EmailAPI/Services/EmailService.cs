using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message= new StringBuilder();
            message.Append("<br/> Cart Email Requested ");
            message.Append("<br/> Total "+ cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name+"*"+ item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");
            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User registration successful.<br/> Email : " + email;
            await LogAndEmail(message, "admin@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try {
                EmailLogger emailLogger = new() { 
                 Email = email,
                 Message = message,
                 EmailSent=DateTime.Now
                };
                await using var _db = new AppDbContext(_dbOptions);
                _db.EmailLoggers.Add(emailLogger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
            return false;
            }
        }
    }
}

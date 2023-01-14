using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace CoderThoughtsBlog.Services.Interfaces
{
    public interface IBlogEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
    }
}

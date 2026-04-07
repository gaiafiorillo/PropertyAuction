using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyAuction.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string toEmail, string code);
        Task SendLoginCode(string toEmail, string code);
    }
}

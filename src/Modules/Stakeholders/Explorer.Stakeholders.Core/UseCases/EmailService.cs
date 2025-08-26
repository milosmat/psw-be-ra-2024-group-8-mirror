using System.Net;
using System.Net.Mail;
using FluentResults;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class EmailService : IEmailService
    {
        private readonly string _host = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _username = "gotravel.group8@gmail.com";
        private readonly string _password = "dreo qwgq uzyi jtnj";
        private readonly bool _enableSsl = true;
        private readonly string _fromEmail = "gotravel.group8@gmail.com";
        /* dreo qwgq uzyi jtnj */

        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        public EmailService(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<Result> SendNewCouponNotificatinToAllTourists(string subject, string body)
        {
            var users = _userRepository.GetUsersByRole(UserRole.Tourist);
            var registeredUsers = users.Where(u => u.IsActive).ToList();

            var userIds = registeredUsers.Select(u => u.Id).ToList();
            var tourists = _personRepository.GetByUserIds(userIds);

            if(tourists == null || tourists.Count == 0)
            {
                return Result.Fail("There are no registered tourists.");
            }
            
            var emailTask = tourists.Select(tourists => SendEmailToUserAsync(tourists.Email, subject, body));

            await Task.WhenAll(emailTask); //pozivi se pokrecu paralelno

            return Result.Ok();
        }
        public async Task<Result> SendEmailToUserAsync(string toEmail,string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_host, _port))
                {
                    client.Credentials = new NetworkCredential(_username, _password);
                    client.EnableSsl = _enableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_fromEmail, "GoTravel"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true  
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }

                return Result.Ok();  
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to send an email: {ex.Message}");
            }
        }

    }
}

using Core.Interfaces;
using Core.Models;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public BackgroundService(IMailService mailService, IBackgroundJobClient backgroundJobClient)
        {
            _mailService = mailService;
            _backgroundJobClient = backgroundJobClient;
        }
        public void SendMailInTheBackground(MailRequest request)
        {
            _backgroundJobClient.Enqueue(() => _mailService.SendEmailAsync(request));
            Console.WriteLine("request has been sent sucessfully");
        }
    }
}

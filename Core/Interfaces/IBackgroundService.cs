using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBackgroundService
    {
        public void SendMailInTheBackground(MailRequest request); 
    }
}

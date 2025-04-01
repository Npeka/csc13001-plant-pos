using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csc13001_plant_pos.Core.Contracts.Services;
public interface INotificationService
{
    Task SendOTP(string receiverEmail, string otp);
}

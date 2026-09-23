using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Contract.Util
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}

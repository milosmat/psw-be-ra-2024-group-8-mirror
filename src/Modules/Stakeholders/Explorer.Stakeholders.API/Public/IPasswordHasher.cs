using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedPasswordHash);
    }
}

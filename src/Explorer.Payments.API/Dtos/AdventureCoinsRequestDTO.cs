using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class AdventureCoinsRequest
    {
        public int IdWallet { get; set; }
        public int IdAdministrator { get; set; }
        public long AdventureCoins { get; set; }
        public string Description { get; set; }
    }
}

using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public.Tourist
{
    public interface IWalletService
    {
      //  Result<WalletDTO> AddTransaction(WalletDTO walletDTO);
       // Result<WalletDTO> SubtractTransaction (WalletDTO walletDTO);
        //Result<WalletDTO> GetTransaction (WalletDTO walletDTO);
        Result<WalletDTO> Get (int id);
        Result Delete(int id);
        Result<WalletDTO> Update (WalletDTO walletDTO);
        Result<WalletDTO> Create (WalletDTO walletDTO);

    }
}

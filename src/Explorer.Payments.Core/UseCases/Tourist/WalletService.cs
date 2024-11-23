using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class WalletService : CrudService<WalletDTO, Wallet>, IWalletService
    {
        private readonly ICrudRepository<Wallet> _walletRepository;
        private readonly ICrudRepository<Transaction> _transactionRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IMapper _mapper;
        public WalletService(ICrudRepository<Wallet> crudRepository, IMapper mapper, IWalletRepository walletRepository) : base(crudRepository, mapper)
        {
            _mapper = mapper;
            _walletRepository = crudRepository;
            this.walletRepository = walletRepository;
        }

        public Result<WalletDTO> Get(int id)
        {
            try
            {
                var walletResult = walletRepository.Get(id);

                if (walletResult == null)
                {
                    return null;
                }

                WalletDTO walletDTO = new WalletDTO
                {
                    Id = (int)walletResult.Id,
                    TouristId = (long) walletResult.TouristId,
                    AdventureCoins = walletResult.AdventureCoins,                   
                    Transactions = walletResult.Transactions.Select(t => new WalletDTO.TransactionItemsDTO
                    {
                        AdministratorId = t.AdministratorId,
                        Amount = t.Amount,
                        Description = t.Description,
                        TransactionTime = t.TransactionTime

                    }).ToList()
                };

                return walletDTO;

            }catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        /*
        Result Delete(int id);
        Result<WalletDTO> Update(WalletDTO walletDTO);
        Result<WalletDTO> Create(WalletDTO walletDTO);

        */
    }
}

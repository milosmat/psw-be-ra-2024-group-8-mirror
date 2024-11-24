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
using static Explorer.Payments.API.Dtos.WalletDTO;
using Transaction = Explorer.Payments.Core.Domain.Transaction;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class WalletService : CrudService<WalletDTO, Wallet>, IWalletService
    {
        private readonly ICrudRepository<Wallet> _walletRepository;
        private readonly ICrudRepository<Transaction> _transactionRepository;
        private readonly IWalletRepository walletRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper _mapper;
        public WalletService(ICrudRepository<Wallet> crudRepository, IMapper mapper, IWalletRepository walletRepository, ITransactionRepository transactionRepository) : base(crudRepository, mapper)
        {
            _mapper = mapper;
            _walletRepository = crudRepository;
            this.walletRepository = walletRepository;
            this.transactionRepository = transactionRepository;
        }

        public Result<WalletDTO> Get(int id)
        {
            try
            {
                var walletResult = walletRepository.Get(id);
                List<Transaction>? transactionResult = transactionRepository.GetTransactions(id); // TO DO proveriti preko cijeg ID se ovo trazi
                List<TransactionItemsDTO>? tranactionList = new List<TransactionItemsDTO>();

                if (walletResult == null)
                {
                    return null;
                }
                
                if (transactionResult != null)
                {
                    foreach (var t in transactionResult)
                    {
                        TransactionItemsDTO transactionItemsDTO = new TransactionItemsDTO(
                            t.Amount,
                            t.Description,
                            t.TransactionTime,
                            t.WalletId
                            );
                        tranactionList.Add( transactionItemsDTO );
                    }
                }

                WalletDTO walletDTO = new WalletDTO
                {
                    Id = (int)walletResult.Id,
                    TouristId = (long)walletResult.TouristId,
                    AdventureCoins = walletResult.AdventureCoins,
                    Transactions = tranactionList
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

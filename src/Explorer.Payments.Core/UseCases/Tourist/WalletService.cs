using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
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
        private readonly IMessageRepository messageRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IMapper _mapper;
        public WalletService(ICrudRepository<Wallet> crudRepository, IMapper mapper, IWalletRepository walletRepository,
            ITransactionRepository transactionRepository, INotificationRepository notificationRepository,
            IMessageRepository messageRepository) : base(crudRepository, mapper)
        {
            _mapper = mapper;
            _walletRepository = crudRepository;
            this.walletRepository = walletRepository;
            this.transactionRepository = transactionRepository;
            this.notificationRepository = notificationRepository;
            this.messageRepository = messageRepository;
        }

        public Result<WalletDTO> Get(int id)
        {
            try
            {
                var walletResult = walletRepository.GetWallet(id);
                List<TransactionItemsDTO>? tranactionList = new List<TransactionItemsDTO>();

                if (walletResult == null)
                {
                    return null;
                }
                
                if (walletResult.Transactions != null)
                {
                    foreach (var t in walletResult.Transactions)
                    {
                        TransactionItemsDTO transactionItemsDTO = new TransactionItemsDTO(
                            t.Amount,
                            t.Description,
                            t.TransactionTime,
                            t.AdministratorId,
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
        
        public Result<WalletDTO> AddTransaction(int idWallet,int idAdministrator, long adventureCoins, String description)
        {
            try
            {
                Wallet walletResult = walletRepository.GetWallet(idWallet);

                walletResult.AddTransaction(idAdministrator, adventureCoins, description, idWallet);

                walletRepository.UpdateWallet(walletResult);

                Wallet resultWallet = walletRepository.GetWallet(idWallet);

                List<TransactionItemsDTO>? tranactionList = new List<TransactionItemsDTO>();

                if (resultWallet.Transactions != null)
                {
                    foreach (var t in walletResult.Transactions)
                    {
                        TransactionItemsDTO transactionItemsDTO = new TransactionItemsDTO(
                            t.Amount,
                            t.Description,
                            t.TransactionTime,
                            t.AdministratorId,
                            t.WalletId
                            );
                        tranactionList.Add(transactionItemsDTO);
                    }
                }

                WalletDTO walletDTO = new WalletDTO
                {
                    Id = (int)resultWallet.Id,
                    TouristId = (long)resultWallet.TouristId,
                    AdventureCoins = resultWallet.AdventureCoins,
                    Transactions = tranactionList
                };

                //Slanje notifikacije turisti
                var message = new Message(idAdministrator, description,null,null);
                messageRepository.Create(message);
                var notification = new Notification(idAdministrator, (int)resultWallet.TouristId, message.Id);
                notificationRepository.Create(notification);

                return Result.Ok(walletDTO);

            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.Internal).WithError(e.Message);
            }
        }


        public Result<WalletDTO> SubtractTransaction(int idWallet, int idAdministrator, long adventureCoins, String description)
        {
            try
            {
                Wallet walletResult = walletRepository.GetWallet(idWallet);

                walletResult.SubtractTransaction(idAdministrator, adventureCoins, description, idWallet);

                walletRepository.UpdateWallet(walletResult);

                Wallet resultWallet = walletRepository.GetWallet(idWallet);

                List<TransactionItemsDTO>? tranactionList = new List<TransactionItemsDTO>();

                if (resultWallet.Transactions != null)
                {
                    foreach (var t in walletResult.Transactions)
                    {
                        TransactionItemsDTO transactionItemsDTO = new TransactionItemsDTO(
                            t.Amount,
                            t.Description,
                            t.TransactionTime,
                            t.AdministratorId,
                            t.WalletId
                            );
                        tranactionList.Add(transactionItemsDTO);
                    }
                }

                WalletDTO walletDTO = new WalletDTO
                {
                    Id = (int)resultWallet.Id,
                    TouristId = (long)resultWallet.TouristId,
                    AdventureCoins = resultWallet.AdventureCoins,
                    Transactions = tranactionList
                };

                //Slanje notifikacije turisti
                var message = new Message(idAdministrator, description, null, null);
                messageRepository.Create(message);
                var notification = new Notification(idAdministrator, (int)resultWallet.TouristId, message.Id);
                notificationRepository.Create(notification);

                return Result.Ok(walletDTO);

            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.Internal).WithError(e.Message);
            }
        }

        public Result CreateWallet(long idTourist)
        {
            try
            {
                Wallet newWallet = new Wallet(idTourist, 0, null);
                walletRepository.Create(newWallet);


                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.Internal).WithError(e.Message);
            }
        }

        public long GetWalletIdByTouristId(int id)
        {
            Wallet wallet = walletRepository.GetWalletByTouristId(id);
           
            return wallet.Id;
        }
    }
}

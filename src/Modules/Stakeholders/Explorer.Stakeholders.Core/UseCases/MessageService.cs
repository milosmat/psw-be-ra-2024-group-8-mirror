using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain; // za Core.Domain.ResourceType
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.Clubs;

namespace Explorer.Stakeholders.Core.UseCases;

    public class MessageService : BaseService<MessageDto, Message>, IMessageService
    {

        public IMessageRepository _messageRepository { get; set; }
        private readonly IClubRepository _clubRepository;

    public IMapper _mapper { get; set; }

        public MessageService(IMessageRepository messageRepository, IMapper mapper, IClubRepository clubRepository) : base(mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _clubRepository = clubRepository;
        }

        public PagedResult<MessageDto> GetPagedByClub(long clubId, int page, int pageSize)
        {
            var club = _clubRepository.GetClubWithMessages(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }

            var allMessages = club.Messages?.ToList();
            int totalCount = allMessages.Count;

            if (page != 0 || pageSize != 0)
            {
                allMessages
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            var messagesDto = _mapper.Map<List<MessageDto>>(allMessages);
            return new PagedResult<MessageDto>(messagesDto, totalCount);

    }

    public MessageDto Update(long clubId, MessageDto updateMessage)
    {
        var club = _clubRepository.GetClubWithMessages(clubId);
        if (club == null)
        {
            throw new KeyNotFoundException($"Club with ID {clubId} not found.");
        }

        var updatedMessage = club.UpdateMesagge(_mapper.Map<Message>(updateMessage));
        _clubRepository.Update(club);
        return _mapper.Map<MessageDto>(updatedMessage);
    }

    public void Delete(long clubId, int messageId)
    {
        var club = _clubRepository.GetClubWithMessages(clubId);
        if (club == null)
        {
            throw new KeyNotFoundException($"Club with ID {clubId} not found.");
        }
        club.DeleteMessage(messageId);
        _clubRepository.Update(club);
    }

    public MessageDto GetById(long clubId, long messageId)
    {
        var club = _clubRepository.GetClubWithMessages(clubId);
        if (club == null)
        {
            throw new KeyNotFoundException($"Club with ID {clubId} not found.");
        }

        var message = club.Messages.FirstOrDefault(mr => mr.Id == messageId);
        if (message == null)
        {
            throw new KeyNotFoundException($"Membership request with ID {messageId} not found.");
        }

        return _mapper.Map<MessageDto>(message);

    }
    public MessageDto Create(long clubId, MessageDto newMessage)
    {
        var club = _clubRepository.GetClubWithMessages(clubId);
        if (club == null)
        {
            throw new KeyNotFoundException($"Club with ID {clubId} not found.");
        }

        club.AddMessage(_mapper.Map<Message>(newMessage));
        _clubRepository.Update(club);
        var lastMessage = club.Messages.Last();
        newMessage.Id = (int)lastMessage.Id;
        return newMessage;

    }

}


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

namespace Explorer.Stakeholders.Core.UseCases;

    public class MessageService : BaseService<MessageDto, Message>, IMessageService
    {

        public IMessageRepository _messageRepository { get; set; }
        public IMapper _mapper { get; set; }

        public MessageService(IMessageRepository messageRepository, IMapper mapper) : base(mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public PagedResult<MessageDto> GetPaged(int page, int pageSize)
        {
            PagedResult<Message> messages = _messageRepository.GetPaged(page, pageSize);

            var messageDtos = _mapper.Map<List<MessageDto>>(messages.Results);
            return new PagedResult<MessageDto>(messageDtos, messages.TotalCount);
        }

        public MessageDto Update(MessageDto updateMessage)
        {
            return _mapper.Map<MessageDto>(_messageRepository.Update(_mapper.Map<Message>(updateMessage)));
        }

        public MessageDto Get(int id)
        {
            return _mapper.Map<MessageDto>(_messageRepository.Get(id));
        }

        public void Delete(int id)
        {
            _messageRepository.Delete(id);
        }
}


using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface IMessageService
{
   // Task<Result<MessageDto>> GetMessageByIdAsync(int messageId);
   // Task<Result<List<MessageDto>>> GetMessagesBySenderIdAsync(int senderId);
   // Task<Result<MessageDto>> SendMessageAsync(int senderId, string content, int? resourceId, ResourceType? resourceType);
    PagedResult<MessageDto> GetPaged(int page, int pageSize);
    MessageDto Update(MessageDto updateMessage);

    void Delete(int id);
    MessageDto Get(int id);

}


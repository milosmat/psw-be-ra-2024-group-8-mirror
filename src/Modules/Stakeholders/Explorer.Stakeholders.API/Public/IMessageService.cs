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
    PagedResult<MessageDto> GetPagedByClub(long clubId, int page, int pageSize);
    MessageDto Update(long clubId, MessageDto updateMessage);

    void Delete(long clubId, int id);
    MessageDto GetById(long clubId, long messageId);
    MessageDto Create(long clubId, MessageDto newMessage);

}


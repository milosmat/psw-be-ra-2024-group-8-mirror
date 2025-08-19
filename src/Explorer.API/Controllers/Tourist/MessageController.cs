using Azure.Core;
using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceType = Explorer.Stakeholders.API.Dtos.ResourceType;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/club/{clubId}/message")]

    public class MessageController : BaseApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult<PagedResult<MessageDto>> GetAll([FromRoute] long clubId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = _messageService.GetPagedByClub(clubId, page, pageSize);

                if (result == null || !result.Results.Any())
                {
                     return Ok(new { results = new List<Message>() });
                }
                return Ok(result);


            }
            catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
                    }
        }

        [HttpGet("getAllByOwnerId")]
        public ActionResult<PagedResult<MessageDto>> GetAllByOwnerId([FromRoute] long clubId)
        {
            try
            {
                // Dohvatanje svih poruka
                var result = _messageService.GetPagedByClub(clubId, 1, 10);

                if (result == null || result.Results.Count == 0)
                {
                    return NotFound("No message found for the specified page.");
                }

                // Filtriranje poruka prema ownerId i resourceType
                var filteredResults = result.Results.Where(m => m.OwnerId == clubId && m.ResourceType == ResourceType.Club).ToList();

                if (filteredResults.Count == 0)
                {
                    return NotFound("No messages found for the specified ownerId and resourceType 'Club'.");
                }

                // Kreiramo novi PagedResult sa filtriranim rezultatima
                var filteredResult = new PagedResult<MessageDto>(filteredResults, filteredResults.Count);

                return Ok(filteredResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult<MessageDto> Update([FromRoute] long clubId, long id, [FromBody] MessageDto message)
        {
            try
            {

                var existingMemRequest = _messageService.GetById(clubId,id);

                if (existingMemRequest == null)
                {
                    return NotFound($"Membership request with ID {id} not found.");
                }
                
                var result = _messageService.Update(clubId, message);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromRoute] long clubId, int id)
        {
            var message = _messageService.GetById(clubId, id);

            if (message == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            _messageService.Delete(clubId, id);

            return Ok($"Blog with ID {id} has been successfully deleted.");
        }

        [HttpPost]
        public ActionResult<MessageDto> Create([FromRoute] long clubId, [FromBody] MessageDto message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message.Content) || message.SenderId == 0)
                {
                    return BadRequest("Content cannot be null or empty");
                }

                var createdMessage = _messageService.Create(clubId, message);
                return CreatedAtAction(nameof(Create), new { clubId, messageId = createdMessage.Id }, createdMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}

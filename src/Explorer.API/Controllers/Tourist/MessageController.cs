using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/message")]
    public class MessageController : BaseApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult<PagedResult<MessageDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = _messageService.GetPaged(page, pageSize);

                if (result == null || result.Results.Count == 0)
                {
                    return NotFound("No message found for the specified page.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
            }
        }

        [HttpGet("{ownerId:int}")]
        public ActionResult<PagedResult<MessageDto>> GetAllByOwnerId([FromRoute] long ownerId)
        {
            try
            {
                // Dohvatanje svih poruka
                var result = _messageService.GetPaged(1, 10);

                if (result == null || result.Results.Count == 0)
                {
                    return NotFound("No message found for the specified page.");
                }

                // Filtriranje poruka prema ownerId i resourceType
                var filteredResults = result.Results.Where(m => m.OwnerId == ownerId && m.ResourceType == ResourceType.Club).ToList();

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
        public ActionResult<BlogsDto> Update([FromBody] MessageDto message)
        {
            try
            {
                MessageDto result = _messageService.Update(message);
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Return a 500 error for unexpected issues
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var message = _messageService.Get(id);

            if (message == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            _messageService.Delete(id);

            return Ok($"Blog with ID {id} has been successfully deleted.");
        }

    }
}

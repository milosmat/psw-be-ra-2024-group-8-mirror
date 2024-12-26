using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class MessageCommandTests : BaseStakeholdersIntegrationTest
    {
        public MessageCommandTests(StakeholdersTestFactory factory) : base(factory) { }



        [Theory]
        [InlineData(-22, "Kreirana poruka", -1, ResourceType.Club)] // SenderId, OwnerId, Status, OwnerId
        public void CreatesMessage(int senderId, string content, int ownerId, ResourceType status)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var newMessage = new MessageDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                ResourceType = status,
                Content = content,
            };

            // Act
            var result = ((ObjectResult)controller.Create(ownerId, newMessage).Result)?.Value as MessageDto;
            var createdMembershipRequestId = result?.Id ?? 0;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.SenderId.ShouldBe(newMessage.SenderId);
            result.OwnerId.ShouldBe(newMessage.OwnerId);
            result.ResourceType.ShouldBe(newMessage.ResourceType);
            result.Content.ShouldBe(newMessage.Content);

            // Assert - Database
            var storedEntity = dbContext.Messages.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.SenderId.ShouldBe(newMessage.SenderId);
            storedEntity.OwnerId.ShouldBe(newMessage.OwnerId);
            //  storedEntity.Status.ShouldBe(newMembershipRequest.Status);
            storedEntity.Content.ShouldBe(newMessage.Content);
        }

        [Theory]
        [InlineData(-22, null, -1, ResourceType.Club, 400)] // SenderId, OwnerId, Status, OwnerId
        [InlineData(0, "Neispravna poruka", -1, ResourceType.Club, 400)] // SenderId, OwnerId, Status, OwnerId
        public void CreateMessage_Fails_InvalidData(int senderId, string content, int ownerId, ResourceType status, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var newMessage = new MessageDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                ResourceType = status,
                Content = content,
            };

            // Act
            var result = (ObjectResult)controller.Create(ownerId, newMessage).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-23, "Kreirana poruka za azuriranje", -2, ResourceType.Club)] // SenderId, OwnerId, Status, OwnerId
        public void Updates(int senderId, string content, int ownerId, ResourceType status)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Kreiranje početnog MembershipRequest objekta
            var newMessage = new MessageDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                ResourceType = status,
                Content = content,
            };

            var createdMessage = ((ObjectResult)controller.Create(ownerId, newMessage).Result)?.Value as MessageDto;
            var createdMessageId = createdMessage?.Id ?? -1;

            // Ažurirani MembershipRequest objekat
            var updatedMessage = new MessageDto
            {
                Id = createdMessageId,
                SenderId = senderId,
                OwnerId = ownerId,
                ResourceType = status,
                Content = "Updatovana poruka",
            };

            // Act
            var result = ((ObjectResult)controller.Update(ownerId, createdMessageId, updatedMessage).Result)?.Value as MessageDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(createdMessageId);
            result.Content.ShouldBe(updatedMessage.Content);

            // Assert - Database
            var storedEntity = dbContext.Messages.FirstOrDefault(i => i.Id == createdMessageId);
            storedEntity.ShouldNotBeNull();
        }


        [Theory]
        [InlineData(-1000, -1000, "Kreirana poruka za azuriranje", -1000, ResourceType.Blog, 404)] // SenderId, OwnerId, Status, OwnerId
        public void Update_fails_invalid_id(int id, int senderId, string content, int ownerId, ResourceType status, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var updatedMessage = new MessageDto
            {
                Id = id,
                ResourceType = status,
            };

            // Act
            var result = (ObjectResult)controller.Update(ownerId, id, updatedMessage).Result;
            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-2, true)]
        public void DeletesMessage(int id, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var preDeleteEntity = dbContext.Messages.FirstOrDefault(c => c.Id == id);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act - Pozivamo metodu za brisanje
            controller.Delete(-2, id);

            // Assert - Proveravamo da li je entitet obrisan iz baze
            var postDeleteEntity = dbContext.Messages.FirstOrDefault(i => i.Id == id);
            postDeleteEntity.ShouldNotBeNull(); // Trebalo bi da je null ako je uspešno obrisan ili nije postojao
            //postDeleteEntity..ShouldBe(null);

        }

        [Theory]
        [InlineData(-1000)] // Invalid membership ID
        public void Delete_throws_KeyNotFoundException_for_invalid_id(int membershipId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => controller.Delete(-3, membershipId));

            // Optional: Verify the exception message
            exception.Message.ShouldBe($"Message with ID { membershipId} not found.");
        }


        private static MessageController CreateController(IServiceScope scope)
        {
            return new MessageController(scope.ServiceProvider.GetRequiredService<IMessageService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

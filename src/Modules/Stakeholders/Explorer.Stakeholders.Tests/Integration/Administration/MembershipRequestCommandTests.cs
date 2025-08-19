using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MemRequestStatus = Explorer.Stakeholders.API.Dtos.MemRequestStatus;


namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class MembershipRequestCommandTests : BaseStakeholdersIntegrationTest
    {
        public MembershipRequestCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(-11, -23, MemRequestStatus.Pending, -1)] // SenderId, OwnerId, Status, OwnerId
        public void CreatesMembershipRequest(int senderId, int ownerId, MemRequestStatus status, long clubId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var newMembershipRequest = new MembershipRequestDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                Status = status,
                ClubId = clubId,
            };

            // Act
            var result = ((ObjectResult)controller.Create(clubId, newMembershipRequest).Result)?.Value as MembershipRequestDto;
            var createdMembershipRequestId = result?.Id ?? 0;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.SenderId.ShouldBe(newMembershipRequest.SenderId);
            result.OwnerId.ShouldBe(newMembershipRequest.OwnerId);
            result.Status.ShouldBe(newMembershipRequest.Status);
            result.ClubId.ShouldBe(newMembershipRequest.ClubId);

            // Assert - Database
            var storedEntity = dbContext.MembershipRequests.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.SenderId.ShouldBe(newMembershipRequest.SenderId);
            storedEntity.OwnerId.ShouldBe(newMembershipRequest.OwnerId);
          //  storedEntity.Status.ShouldBe(newMembershipRequest.Status);
            storedEntity.ClubId.ShouldBe(newMembershipRequest.ClubId);
        }

        [Theory]
        [InlineData(-12, -23, MemRequestStatus.Pending, null, 404)] // OwnerId null
        [InlineData(-13, -21, MemRequestStatus.Pending, -10, 404)] // Club does not exist
        public void CreateMembershipRequest_Fails_InvalidData(int senderId, int ownerId, MemRequestStatus status, int clubId, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var newMembershipRequest = new MembershipRequestDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                Status = status,
                ClubId = clubId,
            };

            // Act
            var result = (ObjectResult)controller.Create(clubId, newMembershipRequest).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(MemRequestStatus.Accepted, -1L, -22, -11)]
        public void Updates(MemRequestStatus status, long clubId, int senderId, int ownerId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Kreiranje početnog MembershipRequest objekta
            var newMembershipRequest = new MembershipRequestDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                Status = MemRequestStatus.Pending,
                ClubId = clubId,
            };

            var createdMembershipRequest = ((ObjectResult)controller.Create(clubId, newMembershipRequest).Result)?.Value as MembershipRequestDto;
            var createdMembershipRequestId = createdMembershipRequest?.Id ?? -1;

            // Ažurirani MembershipRequest objekat
            var updatedMembershipRequest = new MembershipRequestDto
            {
                Id = createdMembershipRequestId,
                SenderId = senderId,
                OwnerId = ownerId,
                Status = status,
                ClubId = clubId,
            };

            // Act
            var result = ((ObjectResult)controller.Update(clubId, createdMembershipRequestId, updatedMembershipRequest).Result)?.Value as MembershipRequestDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(createdMembershipRequestId);
            result.Status.ShouldBe(updatedMembershipRequest.Status);

            // Assert - Database
            var storedEntity = dbContext.MembershipRequests.FirstOrDefault(i => i.Id == createdMembershipRequestId);
            storedEntity.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(-1000,-1000, MemRequestStatus.Accepted, 404)] // Invalid club ID
        public void Update_fails_invalid_id(long id,long clubId, MemRequestStatus status, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var updatedMemshipRequest = new MembershipRequestDto
            {
                Id = id,
                Status = status,
            };

            // Act
            var result = (ObjectResult)controller.Update(clubId, id, updatedMemshipRequest).Result;
            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-15, -10, MemRequestStatus.Pending, 404)] // Invalid MembershipRequest data
        public void Update_fails_invalid_content(int senderId, int ownerId, MemRequestStatus status, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var newMembershipRequest = new MembershipRequestDto
            {
                SenderId = senderId,
                OwnerId = ownerId,
                Status = MemRequestStatus.Pending,
                ClubId = -10L
            };

            var createdMembershipRequest = ((ObjectResult)controller.Create(-10L, newMembershipRequest).Result)?.Value as MembershipRequestDto;
            var createdMembershipRequestId = createdMembershipRequest?.Id ?? -1;

            var updatedMembershipRequest = new MembershipRequestDto
            {
                Id = createdMembershipRequestId,
                SenderId = senderId,
                OwnerId = ownerId,
                Status = status,
                ClubId = -10L
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedMembershipRequest.ClubId, updatedMembershipRequest.Id, updatedMembershipRequest).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-3, true)]
        public void DeletesMembership(int id, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var preDeleteEntity = dbContext.MembershipRequests.FirstOrDefault(c => c.Id == id);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act - Pozivamo metodu za brisanje
            controller.Delete(-3, id);

            // Assert - Proveravamo da li je entitet obrisan iz baze
            var postDeleteEntity = dbContext.MembershipRequests.FirstOrDefault(i => i.Id == id);
            postDeleteEntity.ShouldBeNull(); // Trebalo bi da je null ako je uspešno obrisan ili nije postojao
        }

        [Theory]
        [InlineData(-1000, 404)] // Invalid membership ID
        public void Delete_fails_invalid_id(int membershipId, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1, membershipId);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-3, -21, true)] // Test sa turistom koji je pozvan
        [InlineData(-2, -21, false)]
        public void IsTouristInvited_ReturnsExpectedResult(int clubId, int touristId, bool expectedResult)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Ako očekujemo da je turist pozvan, kreiramo zahtev za članstvo
            if (expectedResult)
            {
                var newMembershipRequest = new MembershipRequestDto
                {
                    SenderId = touristId, // ID turiste
                    OwnerId = -13,        // Neka vrednost vlasnika
                    Status = MemRequestStatus.Invited,
                    ClubId = clubId
                };

                var createResult = (ObjectResult)controller.Create(clubId, newMembershipRequest).Result;
                createResult.ShouldNotBeNull();
                createResult.StatusCode.ShouldBe(201); // Proveravamo da je zahtev uspešno kreiran
            }

            // Act - Pozivamo metodu IsTouristInvited
            var resultInvited = (ObjectResult)controller.IsTouristInvited(clubId, touristId).Result;

            // Assert - Proveravamo da li rezultat odgovara očekivanom
            resultInvited.ShouldNotBeNull();
            resultInvited.StatusCode.ShouldBe(200); // OK status code
            var isInvited = (bool)resultInvited.Value;
            isInvited.ShouldBe(expectedResult); // Provera da li je turist pozvan
        }



        [Theory]
        [InlineData(-1, -23, -11)] 
        public void AddMembershipRequest_AddsRequestCorrectly(int clubId, int senderId, int ownerId)
        {
            // Arrange
            var club = GetTestClub(clubId,ownerId);

            var newRequest = new MembershipRequest(senderId, ownerId, Core.Domain.Clubs.MemRequestStatus.Invited);

            // Act
            club.AddMembershipRequest(newRequest);

            // Assert
            var request = club.MembershipRequests.FirstOrDefault(mr => mr.SenderId == senderId && mr.OwnerId == ownerId && mr.ClubId == clubId);
            request.ShouldNotBeNull();
        }

        [Fact]
        public void AddMembershipRequest_NullRequestThrowsException()
        {
            // Arrange
            var club = GetTestClub(-1,-11);

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => club.AddMembershipRequest(null));
        }
        [Fact]
        public void DeleteMembershipRequest_RemovesRequestCorrectly()
        {
            // Arrange
            var club = GetTestClub(-2,-12);
            var request = new MembershipRequest(-22, -12, Core.Domain.Clubs.MemRequestStatus.Invited);

            club.AddMembershipRequest(request);

            // Act
            club.DeleteMembershipRequest(request.Id);

            // Assert
            var deletedRequest = club.MembershipRequests.FirstOrDefault(mr => mr.Id == request.Id);
            deletedRequest.ShouldBeNull();
        }
        
        [Fact]
        public void DeleteMembershipRequest_RequestNotFoundThrowsException()
        {
            // Arrange
            var club = GetTestClub(-1,-11);

            // Act & Assert
            Should.Throw<KeyNotFoundException>(() => club.DeleteMembershipRequest(999)); // Assuming ID 999 doesn't exist
        }
        
        [Theory]
        [InlineData(-1, -23, -11)]
        public void UpdateMembershipRequest_UpdatesRequestCorrectly(int clubId, int senderId, int ownerId)
        {
            // Arrange
            var club = GetTestClub(clubId,ownerId);
            var newRequest = new MembershipRequest(senderId, ownerId, Core.Domain.Clubs.MemRequestStatus.Invited);
            club.AddMembershipRequest(newRequest);

            var updatedRequest = new MembershipRequest(newRequest.Id,senderId, ownerId, Core.Domain.Clubs.MemRequestStatus.Accepted);

            // Act
            var result = club.UpdateMembershipRequest(updatedRequest);

            // Assert
            result.Status.ShouldBe(Core.Domain.Clubs.MemRequestStatus.Accepted);
            result.Status.ShouldNotBe(Core.Domain.Clubs.MemRequestStatus.Invited);
        }
        
        [Fact]
        public void UpdateMembershipRequest_RequestNotFoundThrowsException()
        {
            // Arrange
            var club = GetTestClub(-1,-11);
            var updatedRequest = new MembershipRequest(999, -23, -11, Core.Domain.Clubs.MemRequestStatus.Accepted);

            // Act & Assert
            Should.Throw<KeyNotFoundException>(() => club.UpdateMembershipRequest(updatedRequest));
        }

        private Club GetTestClub(long id, int ownerId)
        {
            return new Club(id, "Test Name", "Test Description", "Test Photo", ownerId, new List<MembershipRequest>(), new List<Message>());
        }

        private static MembershipRequestContoller CreateController(IServiceScope scope)
        {
            return new MembershipRequestContoller(scope.ServiceProvider.GetRequiredService<IMembershipRequestService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

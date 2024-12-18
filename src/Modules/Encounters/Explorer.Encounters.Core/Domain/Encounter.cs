using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Encounters.Core.Domain
{
    public class Encounter : Entity
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public MapLocation Location { get; init; }
        public int XP { get; private set; }
        public EncounterStatus Status { get; private set; }
        public EncounterType Type { get; init; }
        public DateTime PublishedDate { get; private set; }
        public DateTime ArchivedDate { get; private set; }
        public long AuthorId { get; private set; }
        public string? Image { get; private set; }
        public List<long>? UsersWhoCompletedId { get; private set; }
        public bool IsReviewed { get; private set; }
        public bool? IsRequired { get; private set; }
        public Encounter() { }

        public Encounter(string name, string description, MapLocation location, int xp, EncounterType type,List<long>? users, long authorId, bool isReviewed, string? image = null) { }

        //public Encounter() { }

        public Encounter(string name, string description, MapLocation location, int xp, EncounterType type,List<long>? users, long authorId, bool isReviewed, string? image = null, bool? isRequired = false)
        {
            // Validate name and xp as required
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            if (xp < 0) throw new ArgumentException("XP cannot be negative.");

            Name = name;
            Description = description;
            Location = location;
            XP = xp;
            Type = type;
            Status = EncounterStatus.DRAFT;
            AuthorId = authorId;

            // Samo za hidden location, u suprotnom ce biti null
            Image = image;
            UsersWhoCompletedId = users;
            IsReviewed = isReviewed;
            IsRequired = isRequired;
        }

        public Result MarkAsReviewed()
        {
            if (IsReviewed)
            {
                return Result.Fail("Encounter is already reviewed.");
            }

            IsReviewed = true;
            return Result.Ok();
        }
        // Methods to manage Encounter lifecycle
        public Result Publish()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || Location==null)
                return Result.Fail("Encounter must have a valid name, description, and location to be published.");

            Status = EncounterStatus.ACTIVE;
            PublishedDate = DateTime.UtcNow;
            return Result.Ok();
        }
        public void SetPublishedDateNow()
        {
            PublishedDate = DateTime.UtcNow;
        }
        public void Archive()
        {
            Status = EncounterStatus.ARCHIVED;
            ArchivedDate = DateTime.UtcNow;
        }

        // Methods for updating XP or other details
        public Result UpdateXP(int newXP)
        {
            if (newXP < 0) return Result.Fail("XP cannot be negative.");

            XP = newXP;
            return Result.Ok();
        }
    }

    public enum EncounterStatus
    {
        DRAFT,
        ACTIVE,
        ARCHIVED
    }

    public enum EncounterType
    {
        SOCIAL,
        LOCATION,
        MISC
    }
}

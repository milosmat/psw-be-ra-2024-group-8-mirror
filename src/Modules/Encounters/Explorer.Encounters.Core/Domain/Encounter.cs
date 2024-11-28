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

        // Dodata polja za Social Encounter
        public int? RequiredParticipants { get; private set; } = 0;
        public int? Radius { get; private set; } = 0;

        public Encounter() { }

        public Encounter(string name, string description, MapLocation location, int xp, EncounterType type, List<long>? users, long authorId, string? image = null, int? requiredParticipants = null, int? radius = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            if (xp < 0) throw new ArgumentException("XP cannot be negative.");

            Name = name;
            Description = description;
            Location = location;
            XP = xp;
            Type = type;
            Status = EncounterStatus.DRAFT;
            AuthorId = authorId;
            Image = image;
            UsersWhoCompletedId = users;
            RequiredParticipants = requiredParticipants ?? 0;
            Radius = radius ?? 0;
        }

        public void SetSocialEncounterData(int requiredParticipants, int radius)
        {
            if (Type != EncounterType.SOCIAL)
                throw new InvalidOperationException("This encounter is not of type SOCIAL.");

            if (requiredParticipants <= 0) throw new ArgumentException("Required participants must be greater than 0.");
            if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");

            RequiredParticipants = requiredParticipants;
            Radius = radius;
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

        public void AddUserToCompleted(long userId)
        {
            if (UsersWhoCompletedId == null)
                UsersWhoCompletedId = new List<long>();

            if (!UsersWhoCompletedId.Contains(userId))
                UsersWhoCompletedId.Add(userId);
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

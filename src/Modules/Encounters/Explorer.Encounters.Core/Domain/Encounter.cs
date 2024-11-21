using Explorer.BuildingBlocks.Core.Domain;
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
        public string Location { get; init; }
        public int XP { get; private set; }
        public EncounterStatus Status { get; private set; }
        public EncounterType Type { get; init; }
        public DateTime PublishedDate { get; private set; }
        public DateTime ArchivedDate { get; private set; }

        public long AuthorId { get; private set; }

        public Encounter() { }

        public Encounter(string name, string description, string location, int xp, EncounterType type, long authorId)
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
        }

        // Methods to manage Encounter lifecycle
        public Result Publish()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(Location))
                return Result.Fail("Encounter must have a valid name, description, and location to be published.");

            Status = EncounterStatus.ACTIVE;
            PublishedDate = DateTime.UtcNow;
            return Result.Ok();
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

using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using FluentResults;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;

namespace Explorer.Tours.Core.Domain
{
    public class Tour : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Weight { get; private set; }
        public string[] Tags { get; private set; }
        public TourStatus Status { get; private set; }
        public decimal? Price { get; private set; }
        public long LengthInKm { get; private set; }
        public DateTime PublishedDate { get; private set; }
        public DateTime ArchivedDate { get; private set; }
        // Kolekcije vrednosnih objekata i entiteta
        public List<Equipment> Equipments { get; private set; } = new List<Equipment>();
        public List<TourCheckpoint> TourCheckpoints { get; private set; } = new List<TourCheckpoint>();
        //public List<TravelTime> TravelTimes { get; private set; } = new List<TravelTime>();
        public Tour() { }
        public Tour(string name, string description, string weight, string[] tags, long lengthInKm, DateTime publishedDate, DateTime archivedDate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            Description = description;
            Weight = weight;
            Tags = tags;
            Status = TourStatus.DRAFT;
            Price = 0;
            LengthInKm = lengthInKm;
            PublishedDate = publishedDate;
            ArchivedDate = archivedDate;
        }

        // Metode za upravljanje Equipments (vrednosni objekti)
        public Result AddEquipment(Equipment equipment)
        {

            if (Equipments.Contains(equipment))
                return Result.Fail("Equipment already exists.");

            Equipments.Add(equipment);
            return Result.Ok();
        }

        public Result RemoveEquipment(Equipment equipment)
        {
            if (!Equipments.Contains(equipment))
                return Result.Fail("Equipment not found.");

            Equipments.Remove(equipment);
            return Result.Ok();
        }

        // Metode za upravljanje TourCheckpoints (entiteti)
        public Result AddCheckpoint(TourCheckpoint checkpoint)
        {
            if (TourCheckpoints.Any(c => c.Id == checkpoint.Id))
                return Result.Fail("Checkpoint already exists.");

            TourCheckpoints.Add(checkpoint);
            return Result.Ok();
        }

        public Result RemoveCheckpoint(TourCheckpoint checkpoint)
        {
            if (!TourCheckpoints.Contains(checkpoint))
                return Result.Fail("Checkpoint not found.");

            TourCheckpoints.Remove(checkpoint);
            return Result.Ok();
        }

        // Metode za upravljanje TravelTimes (vrednosni objekti)
/*        public Result AddTravelTime(TravelTime travelTime)
        {
            if (TravelTimes.Contains(travelTime))
                return Result.Fail("Travel time already exists.");

            TravelTimes.Add(travelTime);
            return Result.Ok();
        }

        public Result RemoveTravelTime(TravelTime travelTime)
        {
            if (!TravelTimes.Contains(travelTime))
                return Result.Fail("Travel time not found.");

            TravelTimes.Remove(travelTime);
            return Result.Ok();
        }*/
    }

    public enum TourStatus
    {
        DRAFT, PUBLISHED, ARCHIVED
    }
}
